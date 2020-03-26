using System;
using GraphQl.AspNetCore;
using Microsoft.AspNetCore.Http;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods used to add the middleware to the HTTP request pipeline.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        private static readonly string _defaultPath = "/graphql";

        /// <summary>
        /// Adds a GraphQL middleware to the <see cref="IApplicationBuilder"/> request execution pipeline with default path and options.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseGraphQl(
            this IApplicationBuilder builder)
        {
            return builder.UseGraphQl(null, new GraphQlMiddlewareOptions());
        }

        /// <summary>
        /// Adds a GraphQL middleware to the <see cref="IApplicationBuilder"/> request execution pipeline with default options.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseGraphQl(
            this IApplicationBuilder builder,
            string path)
        {
            return builder.UseGraphQl(path, new GraphQlMiddlewareOptions());
        }

        /// <summary>
        /// Adds a GraphQL middleware to the <see cref="IApplicationBuilder"/> request execution pipeline with a callback to configure options.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="path"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseGraphQl(
            this IApplicationBuilder builder,
            string path,
            Action<GraphQlMiddlewareOptions> configure)
        {
            var options = new GraphQlMiddlewareOptions();
            configure(options);

            return builder.UseGraphQl(path, options);
        }

        /// <summary>
        /// Adds a GraphQL middleware to the <see cref="IApplicationBuilder"/> request execution pipeline with a callback to configure options.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseGraphQl(
            this IApplicationBuilder builder,
            Action<GraphQlMiddlewareOptions> configure)
        {
            var options = new GraphQlMiddlewareOptions();
            configure(options);

            return builder.UseGraphQl(_defaultPath, options);
        }

        /// <summary>
        /// Adds a GraphQL middleware to the <see cref="IApplicationBuilder"/> request execution pipeline with a callback to configure options.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseGraphQl(
            this IApplicationBuilder builder,
            GraphQlMiddlewareOptions options)
        {
            return builder.UseGraphQl(_defaultPath, options);
        }

        /// <summary>
        /// Adds a GraphQL middleware to the <see cref="IApplicationBuilder"/> request execution pipeline with the specified options.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="path"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseGraphQl(
            this IApplicationBuilder builder,
            string path,
            GraphQlMiddlewareOptions options)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (path == null)
                path = _defaultPath;

            var schemaProvider = SchemaConfiguration.GetSchemaProvider(options.SchemaName, builder.ApplicationServices);

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

            return builder.MapWhen(predicate, b => b.UseMiddleware<GraphQlMiddleware>(schemaProvider, options));
        }
    }
}
