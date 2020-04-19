using System;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Builder;
using GraphQL.AspNetCore;
using GraphQL.AspNetCore.GraphiQL;

namespace Microsoft.AspNetCore.Builder
{
    public static class EndpointRouteBuilderExtensions
    {
        private static readonly string _defaultDisplayName = "GraphiQL Middleware";
        private static readonly string _defaultPattern = "/graphiql";

        /// <summary>
        /// Adds a GraphiQL endpoint to the <see cref="IEndpointRouteBuilder"/> with the specified template.
        /// </summary>
        /// <param name="routes">The <see cref="IEndpointRouteBuilder"/> to add the GraphiQL endpoint to.</param>
        /// <returns>A convention routes for the GraphiQL endpoint.</returns>
        public static IEndpointConventionBuilder MapGraphiQL(
            this IEndpointRouteBuilder routes)
        {
            var options = new GraphiQLMiddlewareOptions();
            return routes.MapGraphiQL(_defaultPattern, options);
        }

        /// <summary>
        /// Adds a GraphiQL endpoint to the <see cref="IEndpointRouteBuilder"/> with the specified template.
        /// </summary>
        /// <param name="routes">The <see cref="IEndpointRouteBuilder"/> to add the GraphiQL endpoint to.</param>
        /// <param name="pattern">The URL pattern of the GraphiQL endpoint.</param>
        /// <returns>A convention routes for the GraphiQL endpoint.</returns>
        public static IEndpointConventionBuilder MapGraphiQL(
            this IEndpointRouteBuilder routes,
            string pattern)
        {
            var options = new GraphiQLMiddlewareOptions();
            return routes.MapGraphiQL(_defaultPattern, options);
        }

        /// <summary>
        /// Adds a GraphiQL endpoint to the <see cref="IEndpointRouteBuilder"/> with the specified template.
        /// </summary>
        /// <param name="routes">The <see cref="IEndpointRouteBuilder"/> to add the GraphiQL endpoint to.</param>
        /// <param name="pattern">The URL pattern of the GraphiQL endpoint.</param>
        /// <param name="configure">Configure the endpoint.</param>
        /// <returns>A convention routes for the GraphiQL endpoint.</returns>
        public static IEndpointConventionBuilder MapGraphiQL(
            this IEndpointRouteBuilder routes,
            string pattern,
            Action<GraphiQLMiddlewareOptions> configure)
        {
            var options = new GraphiQLMiddlewareOptions();
            configure(options);

            return routes.MapGraphiQL(pattern, options);
        }

        /// <summary>
        /// Adds a GraphiQL endpoint to the <see cref="IEndpointRouteBuilder"/> with the specified template.
        /// </summary>
        /// <param name="routes">The <see cref="IEndpointRouteBuilder"/> to add the GraphiQL endpoint to.</param>
        /// <param name="configure">Configure the endpoint.</param>
        /// <returns>A convention routes for the GraphiQL endpoint.</returns>
        public static IEndpointConventionBuilder MapGraphiQL(
            this IEndpointRouteBuilder routes,
            Action<GraphiQLMiddlewareOptions> configure)
        {
            var options = new GraphiQLMiddlewareOptions();
            configure(options);

            return routes.MapGraphiQL(_defaultPattern, options);
        }

        /// <summary>
        /// Adds a GraphiQL endpoint to the <see cref="IEndpointRouteBuilder"/> with the specified template.
        /// </summary>
        /// <param name="routes">The <see cref="IEndpointRouteBuilder"/> to add the GraphiQL endpoint to.</param>
        /// <param name="options">The <see cref="GraphiQLMiddlewareOptions"/>.</param>
        /// <returns>A convention routes for the GraphiQL endpoint.</returns>
        public static IEndpointConventionBuilder MapGraphiQL(
            this IEndpointRouteBuilder routes,
            GraphiQLMiddlewareOptions options)
        {
            return routes.MapGraphiQL(_defaultPattern, options);
        }

        /// <summary>
        /// Adds a GraphiQL endpoint to the <see cref="IEndpointRouteBuilder"/> with the specified template.
        /// </summary>
        /// <param name="routes">The <see cref="IEndpointRouteBuilder"/> to add the GraphiQL endpoint to.</param>
        /// <param name="pattern">The URL pattern of the GraphiQL endpoint.</param>
        /// <param name="options">The <see cref="GraphiQLMiddlewareOptions"/>.</param>
        /// <returns>A convention routes for the GraphiQL endpoint.</returns>
        public static IEndpointConventionBuilder MapGraphiQL(
            this IEndpointRouteBuilder routes,
            string pattern,
            GraphiQLMiddlewareOptions options)
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

            return MapGraphiQLCore(routes, pattern, options);
        }

        private static IEndpointConventionBuilder MapGraphiQLCore(
            IEndpointRouteBuilder routes,
            string pattern,
            GraphiQLMiddlewareOptions options)
        {
            var pipeline = routes.CreateApplicationBuilder()
                .UseMiddleware<GraphiQLMiddleware>(options)
                .Build();

            //return builder.Map(path, branch => branch.UseMiddleware<GraphiQLMiddleware>(schemaProvider, options));
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
