using System;
using GraphQL.AspNetCore.Graphiql;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
    public static class GraphiqlMiddlewareExtensions
    {
        public static IApplicationBuilder UseGraphiql(
            this IApplicationBuilder builder,
            PathString path,
            Action<GraphiqlMiddlewareOptions> configure)
        {
            var options = new GraphiqlMiddlewareOptions();
            configure(options);

            return builder.UseGraphiql(path, options);
        }

        public static IApplicationBuilder UseGraphiql(
            this IApplicationBuilder builder,
            PathString path,
            GraphiqlMiddlewareOptions options)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            return builder.Map(path, branch => branch.UseMiddleware<GraphiqlMiddleware>(options));
        }
    }
}
