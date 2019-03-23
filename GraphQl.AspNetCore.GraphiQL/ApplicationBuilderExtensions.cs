using System;
using GraphQL.AspNetCore.GraphiQL;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGraphiQL(
            this IApplicationBuilder builder,
            PathString path,
            Action<GraphiQLMiddlewareOptions> configure)
        {
            var options = new GraphiQLMiddlewareOptions();
            configure(options);

            return builder.UseGraphiQL(path, options);
        }

        public static IApplicationBuilder UseGraphiQL(
            this IApplicationBuilder builder,
            PathString path,
            GraphiQLMiddlewareOptions options)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return builder.Map(path, branch => branch.UseMiddleware<GraphiQLMiddleware>(options));
        }
    }
}
