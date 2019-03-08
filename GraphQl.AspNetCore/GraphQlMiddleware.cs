using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Execution;
using GraphQL.Http;
using GraphQL.Instrumentation;
using GraphQL.Types;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GraphQl.AspNetCore
{
    public class GraphQlMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ISchemaProvider _schemaProvider;

        private readonly GraphQlMiddlewareOptions _options;

        private readonly IDocumentExecuter _executer;

        private readonly IEnumerable<IDocumentExecutionListener> _executionListeners;

        private static (string Name, string Value) _allowHeader = ("Allow", $"{HttpMethods.Get}, {HttpMethods.Post}");

        private static string _contentType = "application/json; charset=utf-8";

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
            _executionListeners = executionListeners ?? Array.Empty<IDocumentExecutionListener>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            
            HttpRequest request = httpContext.Request;
            HttpResponse response = httpContext.Response;

            // GraphQL HTTP only supports GET and POST methods.
            if (request.Method != HttpMethods.Get && request.Method != HttpMethods.Post)
            {
                response.Headers.Add(_allowHeader.Name, _allowHeader.Value);
                response.StatusCode = 405; // MethodNotAllowed
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

            var logger = httpContext.RequestServices.GetService<ILogger<GraphQlMiddleware>>();


            GraphQlParameters parameters = await GetParametersAsync(request);

            ISchema schema = _schemaProvider.Create(httpContext.RequestServices);

            var start = DateTime.UtcNow;
            var result = await _executer.ExecuteAsync(options =>
            {
                options.Schema = schema;
                options.Query = parameters.Query;
                options.OperationName = parameters.OperationName;
                options.Inputs = parameters.Variables.ToInputs();
                options.CancellationToken = httpContext.RequestAborted;
                options.ComplexityConfiguration = _options.ComplexityConfiguration;
                options.UserContext = httpContext;
                options.ExposeExceptions = _options.ExposeExceptions;
                options.EnableMetrics = _options.EnableMetrics;
                if (_options.EnableMetrics)
                {
                    options.FieldMiddleware.Use<InstrumentFieldsMiddleware>();
                }

                ConfigureDocumentExecutionListeners(options, _executionListeners);
            }).ConfigureAwait(false);

            if (_options.EnableMetrics)
            {
                result.EnrichWithApolloTracing(start);
            }

            if (result.Errors?.Count > 0)
            {
                logger.LogError("GraphQL Result {Errors}", result.Errors);
            }

            var writer = new DocumentWriter(indent: _options.FormatOutput);
            var json = writer.Write(result);

            response.StatusCode = 200;
            response.ContentType = _contentType;

            await response.WriteAsync(json);
        }

        private static async Task<GraphQlParameters> GetParametersAsync(HttpRequest request)
        {
            // http://graphql.org/learn/serving-over-http/#http-methods-headers-and-body

            string body = null;
            if (request.Method == HttpMethods.Post)
            {
                // Read request body
                using (var sr = new StreamReader(request.Body))
                {
                    body = await sr.ReadToEndAsync();
                }
            }

            MediaTypeHeaderValue.TryParse(request.ContentType, out MediaTypeHeaderValue contentType);

            GraphQlParameters parameters;

            switch (contentType.MediaType)
            {
                case "application/json":
                    // Parse request as json
                    parameters = JsonConvert.DeserializeObject<GraphQlParameters>(body);
                    break;

                case "application/graphql":
                    // The whole body is the query
                    parameters = new GraphQlParameters {Query = body};
                    break;

                default:
                    // Don't parse anything
                    parameters = new GraphQlParameters();
                    break;
            }

            string query = request.Query["query"];

            // Query string "query" overrides a query in the body
            parameters.Query = query ?? parameters.Query;

            return parameters;
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