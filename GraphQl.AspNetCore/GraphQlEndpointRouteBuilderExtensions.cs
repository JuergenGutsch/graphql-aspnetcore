using GraphQl.AspNetCore;
using System;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.AspNetCore.Builder
{
    public static class GraphQlEndpointRouteBuilderExtensions
    {
        private static readonly string _defaultDisplayName = "GraphQL Middleware";
        private static readonly string _defaultPattern = "/graphql";

        /// <summary>
        /// Adds a GraphQL endpoint to the <see cref="IEndpointRouteBuilder"/> with the specified template.
        /// </summary>
        /// <param name="routes">The <see cref="IEndpointRouteBuilder"/> to add the GraphQL endpoint to.</param>
        /// <returns>A convention routes for the GraphQL endpoint.</returns>
        public static IEndpointConventionBuilder MapGraphQl(
            this IEndpointRouteBuilder routes)
        {
            if (routes == null)
            {
                throw new ArgumentNullException(nameof(routes));
            }

            return MapGraphQlCore(routes, _defaultPattern, null);
        }

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

            if (String.IsNullOrWhiteSpace(pattern))
            {
                pattern = _defaultPattern;
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
        public static IEndpointConventionBuilder MapGraphQl(
            this IEndpointRouteBuilder routes,
            string pattern,
            Action<GraphQlMiddlewareOptions> configure)
        {
            if (routes == null)
            {
                throw new ArgumentNullException(nameof(routes));
            }

            if (String.IsNullOrWhiteSpace(pattern))
            {
                pattern = _defaultPattern;
            }
            
            var options = new GraphQlMiddlewareOptions();
            configure(options);

            return MapGraphQlCore(routes, pattern, options);
        }

        /// <summary>
        /// Adds a GraphQL endpoint to the <see cref="IEndpointRouteBuilder"/> with the specified template.
        /// </summary>
        /// <param name="routes">The <see cref="IEndpointRouteBuilder"/> to add the GraphQL endpoint to.</param>
        /// <param name="pattern">The URL pattern of the GraphQL endpoint.</param>
        /// <param name="options">The <see cref="GraphQlMiddlewareOptions"/> con configure the endpoint</param>
        /// <returns>A convention routes for the GraphQL endpoint.</returns>
        public static IEndpointConventionBuilder MapGraphQl(
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
                pattern = _defaultPattern;
            }

            return MapGraphQlCore(routes, pattern, options);
        }

        private static IEndpointConventionBuilder MapGraphQlCore(
            IEndpointRouteBuilder routes,
            string pattern,
            GraphQlMiddlewareOptions options)
        {
            var schemaProvider = SchemaConfiguration.GetSchemaProvider(
                options.SchemaName, routes.ServiceProvider);

            var pipeline = routes.CreateApplicationBuilder()
                .UseMiddleware<GraphQlMiddleware>(schemaProvider, options)
                .Build();

            //return builder.Map(path, branch => branch.UseMiddleware<GraphQlMiddleware>(schemaProvider, options));
            return routes.Map(pattern, pipeline)
                .WithDisplayName(_defaultDisplayName);
        }
    }

    /// TODO: remove when the next ASP.NET Core dev build is done
    public static class RoutingEndpointConventionBuilderExtensions
    {
        /// <summary>
        /// Sets the <see cref="EndpointBuilder.DisplayName"/> to the provided <paramref name="displayName"/> for all
        /// builders created by <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IEndpointConventionBuilder"/>.</param>
        /// <param name="displayName">The display name.</param>
        /// <returns>The <see cref="IEndpointConventionBuilder"/>.</returns>
        public static IEndpointConventionBuilder WithDisplayName(this IEndpointConventionBuilder builder, string displayName)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Add(b =>
            {
                b.DisplayName = displayName;
            });

            return builder;
        }
    }
}
