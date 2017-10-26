using Microsoft.AspNetCore.Builder;
using System;

namespace GraphQL.AspNetCore.Graphiql
{
    public static class GraphiqlMiddlewareExtensions
    {
        public static IApplicationBuilder UseGraphiql(
            this IApplicationBuilder builder,
            Action<GraphiqlMiddlewareOptions> configure)
        {
            var options = new GraphiqlMiddlewareOptions();
            configure(options);
            return builder.UseGraphiql(options);
        }

        public static IApplicationBuilder UseGraphiql(
            this IApplicationBuilder builder,
            GraphiqlMiddlewareOptions options)
        {
            return builder.UseMiddleware<GraphiqlMiddleware>(options);
        }
    }
}
