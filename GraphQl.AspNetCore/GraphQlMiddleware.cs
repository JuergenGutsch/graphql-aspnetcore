using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Execution;
using GraphQL.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GraphQl.AspNetCore
{
    public class GraphQlMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ISchemaProvider _schemaProvider;

        private readonly GraphQlMiddlewareOptions _options;

        private readonly IDocumentExecuter _executer;

        private readonly IEnumerable<IDocumentExecutionListener> _executionListeners;

        public GraphQlMiddleware(
            RequestDelegate next,
            ISchemaProvider schemaProvider,
            GraphQlMiddlewareOptions options,
            IDocumentExecuter executer,
            IEnumerable<IDocumentExecutionListener> executionListeners)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _schemaProvider = schemaProvider ?? throw new ArgumentNullException(nameof(schemaProvider));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _executer = executer ?? throw new ArgumentNullException(nameof(options));
            _executionListeners = executionListeners ?? new IDocumentExecutionListener[0];
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var logger = httpContext.RequestServices.GetService<ILogger<GraphQlMiddleware>>();

            HttpRequest request = httpContext.Request;
            HttpResponse response = httpContext.Response;

            // GraphQL HTTP only supports GET and POST methods.
            if (request.Method != HttpMethods.Get && request.Method != HttpMethods.Post)
            {
                // Handle CORS issue
                if (request.Method == HttpMethods.Options)
                {
                    response.StatusCode = StatusCodes.Status200OK;
                    response.ContentType = "application/json; charset=utf-8";

                    return;
                }

                response.StatusCode = StatusCodes.Status405MethodNotAllowed;

                return;
            }

            // Check authorization
            if (_options.AuthorizationPolicy != null)
            {
                var authorizationService = httpContext.RequestServices.GetRequiredService<IAuthorizationService>();
                var authzResult =
                    await authorizationService.AuthorizeAsync(httpContext.User, _options.AuthorizationPolicy);

                if (!authzResult.Succeeded)
                {
                    await httpContext.ForbidAsync();
                    return;
                }
            }

            GraphQlParameters parameters = await GetParametersAsync(request);

            ISchema schema = _schemaProvider.Create(httpContext.RequestServices);

            var result = await _executer.ExecuteAsync(options =>
            {
                options.Schema = schema;
                options.Query = parameters.Query;
                options.OperationName = parameters.OperationName;
                options.Inputs = parameters.Variables.ToInputs();
                options.CancellationToken = httpContext.RequestAborted;
                options.ComplexityConfiguration = _options.ComplexityConfiguration;

                if (_options.BuildUserContext != null)
                {
                    options.UserContext = _options.BuildUserContext.Invoke(httpContext).Result;
                }
                else
                {
                    options.UserContext = httpContext;
                }

                options.Root = httpContext;

                options.ExposeExceptions = _options.ExposeExceptions;
                options.ValidationRules = _options.ValidationRules;
                ConfigureDocumentExecutionListeners(options, _executionListeners);
            });

            if (result.Errors?.Count > 0)
            {
                logger.LogError("GraphQL Result {Errors}", result.Errors);
            }

            var writer = new DocumentWriter(indent: _options.FormatOutput);
            var json = writer.Write(result);

            response.StatusCode = StatusCodes.Status200OK;
            response.ContentType = "application/json; charset=utf-8";

            await response.WriteAsync(json);
        }

        private static async Task<GraphQlParameters> GetParametersAsync(HttpRequest request)
        {
            GraphQlParameters parameters = null;

            // http://graphql.org/learn/serving-over-http/#http-methods-headers-and-body
            if (request.Method == HttpMethods.Post)
            {
                MediaTypeHeaderValue.TryParse(request.ContentType, out MediaTypeHeaderValue contentType);

                switch (contentType.MediaType.Value)
                {
                    case "application/json":
                        // Parse request as json
                        var bodyJson = await request.ReadAsString();
                        parameters = JsonConvert.DeserializeObject<GraphQlParameters>(bodyJson);
                        break;

                    case "application/graphql":
                        // The whole body is the query
                        var bodyGraphQL = await request.ReadAsString();
                        parameters = new GraphQlParameters { Query = bodyGraphQL };
                        break;
                    case "multipart/form-data":
                        parameters = await GetGraphQLParametersFromMultipartBody(request, contentType);
                        break;
                    default:
                        // Don't parse anything
                        parameters = new GraphQlParameters();
                        break;
                }

                string query = request.Query["query"];

                // Query string "query" overrides a query in the body
                parameters.Query = query ?? parameters.Query;
            }

            return parameters;
        }

        // Based on protocol https://github.com/jaydenseric/graphql-multipart-request-spec
        private static async Task<GraphQlParameters> GetGraphQLParametersFromMultipartBody(HttpRequest request, MediaTypeHeaderValue contentType)
        {
            var graphqlBody = string.Empty;

            var formAccumulator = default(KeyValueAccumulator);

            var boundary = MultipartRequestHelper.GetBoundary(contentType);

            using (var sr = request.ReadAsStream())
            {
                var reader = new MultipartReader(boundary, sr);

                var section = await reader.ReadNextSectionAsync();

                while (section != null)
                {
                    var hasContentDispositionHeader =
                        ContentDispositionHeaderValue.TryParse(
                            section.ContentDisposition,
                            out ContentDispositionHeaderValue contentDisposition);

                    if (hasContentDispositionHeader)
                    {
                        if (contentDisposition.IsFormDisposition())
                        {
                            formAccumulator = await MultipartRequestHelper.AccumulateForm(formAccumulator, section, contentDisposition);
                        }
                    }

                    section = await reader.ReadNextSectionAsync();
                }
            }

            var formResults = formAccumulator.GetResults();
            var operations = formResults.GetPropertyValue("operations");
            var variables = formResults.GetPropertyValue("map");

            return new GraphQlParameters { Query = operations.ToString(), Variables = JObject.Parse(variables.ToString()) };
        }

        private static void ConfigureDocumentExecutionListeners(ExecutionOptions options,
            IEnumerable<IDocumentExecutionListener> listeners)
        {
            Debug.Assert(listeners != null, "listeners != null");

            var listenerSet = new HashSet<IDocumentExecutionListener>(options.Listeners);
            listenerSet.UnionWith(listeners);

            options.Listeners.Clear();
            foreach (var listener in listenerSet)
            {
                options.Listeners.Add(listener);
            }
        }
    }
}