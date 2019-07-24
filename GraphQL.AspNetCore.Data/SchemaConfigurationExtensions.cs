using System;
using GraphQL.AspNetCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQl.AspNetCore
{
    public static class SchemaConfigurationExtensions
    {
        public static SchemaConfiguration AddEntityFrameworkStores<T>(this SchemaConfiguration schema,
            IServiceCollection serviceCollection, Action<GraphBuilder> configureBuilder) where T: DbContext
        {
            serviceCollection.AddTransient<RootQuery>(s =>
            {
                var dbContext = s.GetRequiredService<T>();

                var graphBuilder = new GraphBuilder(dbContext);
                configureBuilder(graphBuilder);
                var rootQuery = graphBuilder.BuildRootType();

                schema.SetQueryInstance(rootQuery);

                //schema.SetResolver(() => rootQuery)

                return rootQuery; 
            });

            return schema;
        }
    }
}
