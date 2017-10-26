using Microsoft.AspNetCore.Builder;
using System;

namespace GraphQl.AspNetCore
{
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GraphQlMiddlewareExtensions
    {
        public static IApplicationBuilder UseGraphQl(
            this IApplicationBuilder builder,
            Action<GraphQlMiddlewareOptions> configure)
        {
            var options = new GraphQlMiddlewareOptions();
            configure(options);
            return builder.UseGraphQl(options);
        }
        public static IApplicationBuilder UseGraphQl(
            this IApplicationBuilder builder,
            GraphQlMiddlewareOptions options)
        {
            return builder.UseMiddleware<GraphQlMiddleware>(options);
        }
    }
}
