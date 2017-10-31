using System;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQl.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add GraphQL services to the specified <see cref="IServiceCollection">IServiceCollection</see>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure">Action to configure the default schema.</param>
        /// <returns></returns>
        public static IGraphQlBuilder AddGraphQl(this IServiceCollection services, Action<SchemaConfiguration> configure)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            if (configure == null)
                throw new ArgumentNullException(nameof(configure));

            var builder = new GraphQlBuilder(services);

            var schema = new SchemaConfiguration(null);
            configure(schema);

            return builder.AddSchema(schema);
        }
    }
}
