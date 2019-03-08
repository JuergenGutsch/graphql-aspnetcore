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
        private static readonly PathString defaultPath = "/graphql";

        /// <summary>
        /// Adds a GraphQL middleware to the <see cref="IApplicationBuilder"/> request execution pipeline with default path and options.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseGraphQl(this IApplicationBuilder builder)
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
            PathString path)
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
            PathString path,
            Action<GraphQlMiddlewareOptions> configure)
        {
            var options = new GraphQlMiddlewareOptions();
            configure(options);

            return builder.UseGraphQl(path, options);
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
            PathString path,
            GraphQlMiddlewareOptions options)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (path == null)
                path = defaultPath;

            var schemaProvider = SchemaConfiguration.GetSchemaProvider(options.SchemaName, builder.ApplicationServices);

            return builder.Map(path, branch => branch.UseMiddleware<GraphQlMiddleware>(schemaProvider, options));
        }
    }
}
