using Microsoft.AspNetCore.Builder;

namespace GraphQlDemo.Middlewares
{
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class GraphQlMiddlewareExtensions
    {
        public static IApplicationBuilder UseGraphQl(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GraphQlMiddleware>();
        }
    }
}
