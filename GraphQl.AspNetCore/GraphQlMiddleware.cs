using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using GraphQL.Http;
using GraphQL.Types;
using GraphQL;
using System.IO;
using System.Linq;

namespace GraphQl.AspNetCore
{
    public class GraphQlMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly GraphQlMiddlewareOptions _options;

        public GraphQlMiddleware(
            RequestDelegate next,
            GraphQlMiddlewareOptions options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var sent = false;
            if (httpContext.Request.Path.StartsWithSegments(_options.GraphApiUrl))
            {
                using (var sr = new StreamReader(httpContext.Request.Body))
                {
                    var query = await sr.ReadToEndAsync();
                    //var o = JsonConvert.DeserializeObject(query);
                    if (!String.IsNullOrWhiteSpace(query))
                    {
                        var schema = new Schema { Query = _options.RootGraphType }; 

                        var result = await new DocumentExecuter()
                            .ExecuteAsync(options =>
                            {
                                options.Schema = schema;
                                options.Query = query;
                                options.ComplexityConfiguration = _options.ComplexityConfiguration;
                            }).ConfigureAwait(false);

                        await WriteResult(httpContext, result);

                        sent = true;
                    }
                }
            }
            if (!sent)
            {
                await _next(httpContext);
            }
        }

        private async Task WriteResult(HttpContext httpContext, ExecutionResult result)
        {
            var json = new DocumentWriter(indent: _options.FormatOutput).Write(result);

            httpContext.Response.StatusCode = result.Errors?.Any() == true ? 400 : 200;
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(json);
        }
    }
}
