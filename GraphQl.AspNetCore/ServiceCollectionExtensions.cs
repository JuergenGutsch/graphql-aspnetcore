﻿using System;
using GraphQl.AspNetCore;
using GraphQL;

namespace Microsoft.Extensions.DependencyInjection
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

            services.AddScoped<DocumentExecuter>();
            
            var builder = new GraphQlBuilder(services);

            var schema = new SchemaConfiguration(null);
            configure(schema);

            return builder.AddSchema(schema);
        }
    }
}
