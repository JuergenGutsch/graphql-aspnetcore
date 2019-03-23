using System;
using GraphQL.AspNetCore.GraphiQL;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        private static readonly string _defaultPath = "/graphiql";

        public static IApplicationBuilder UseGraphiQL(
            this IApplicationBuilder builder)
        {
            var options = new GraphiQLMiddlewareOptions();

            return builder.UseGraphiQL(_defaultPath, options, null);
        }

        public static IApplicationBuilder UseGraphiQL(
            this IApplicationBuilder builder,
            PathString path)
        {
            var options = new GraphiQLMiddlewareOptions();

            return builder.UseGraphiQL(path, options);
        }

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

            if (path == null)
            {
                path = _defaultPath;
            }

            Func<HttpContext, bool> predicate = c =>
            {
                // If you do provide a PathString, want to handle all of the special cases that 
                // StartsWithSegments handles, but we also want it to have exact match semantics.
                //
                // Ex: /Foo/ == /Foo (true)
                // Ex: /Foo/Bar == /Foo (false)
                return c.Request.Path.StartsWithSegments(path, out var remaining) &&
                            string.IsNullOrEmpty(remaining);
            };

            return builder.MapWhen(predicate, branch => branch.UseMiddleware<GraphiQLMiddleware>(options));
        }
    }
}
