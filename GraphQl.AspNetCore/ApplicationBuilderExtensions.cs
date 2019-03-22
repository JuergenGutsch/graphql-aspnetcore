using System;
using GraphQl.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods used to add the middleware to the HTTP request pipeline.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        private const string DefaultDisplayName = "GraphQL Middleware";

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
    }

    public static class GraphQlEndpointRouteBuilderExtensions
    {
        private static readonly PathString _defaultPath = "/graphql";

        /// <summary>
        /// Adds a GraphQL endpoint to the <see cref="IEndpointRouteBuilder"/> with the specified template.
        /// </summary>
        /// <param name="routes">The <see cref="IEndpointRouteBuilder"/> to add the GraphQL endpoint to.</param>
        /// <param name="pattern">The URL pattern of the GraphQL endpoint.</param>
        /// <returns>A convention routes for the GraphQL endpoint.</returns>
        public static IEndpointConventionBuilder MapGraphQl(
            this IEndpointRouteBuilder routes,
            string pattern)
        {
            if (routes == null)
            {
                throw new ArgumentNullException(nameof(routes));
            }

            return MapGraphQlCore(routes, pattern, null);
        }

        /// <summary>
        /// Adds a GraphQL endpoint to the <see cref="IEndpointRouteBuilder"/> with the specified template.
        /// </summary>
        /// <param name="routes">The <see cref="IEndpointRouteBuilder"/> to add the GraphQL endpoint to.</param>
        /// <param name="pattern">The URL pattern of the GraphQL endpoint.</param>
        /// <param name="options">The <see cref="GraphQlMiddlewareOptions"/> con configure the endpoint</param>
        /// <returns>A convention routes for the GraphQL endpoint.</returns>
        public static IApplicationBuilder MapGraphQl(
            this IEndpointRouteBuilder routes,
            string pattern,
            GraphQlMiddlewareOptions options)
        {
            if (routes == null)
            {
                throw new ArgumentNullException(nameof(routes));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (String.IsNullOrWhiteSpace(pattern))
            {
                path = _defaultPath;
            }

            var schemaProvider = SchemaConfiguration.GetSchemaProvider(options.SchemaName, builder.ApplicationServices);

            //return builder.Map(path, branch => branch.UseMiddleware<GraphQlMiddleware>(schemaProvider, options));
            return MapGraphQl(routes, pattern, schemaProvider);
        }

        private static IEndpointConventionBuilder MapGraphQl(
            IEndpointRouteBuilder routes,
            string pattern,
            ISchemaProvider schemaProvider)
        {
            // if (routes.ServiceProvider.GetService(typeof(GraphQlervice)) == null)
            // {
            //     throw new InvalidOperationException(Resources.FormatUnableToFindServices(
            //         nameof(IServiceCollection),
            //         nameof(GraphQlerviceCollectionExtensions.AddGraphQl),
            //         "ConfigureServices(...)"));
            // }

            var pipeline = routes.CreateApplicationBuilder()
               .UseMiddleware<GraphQlMiddleware>(schemaProvider, options)
               .Build();

            return routes.Map(pattern, pipeline).WithDisplayName(DefaultDisplayName);
        }
    }
}
