using System;
using System.Linq;
using GraphQL.AspNetCore.Data;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQl.AspNetCore
{
    public static class SchemaConfigurationExtensions
    {
        public static SchemaConfiguration AddEntityFrameworkStores<T>(this SchemaConfiguration schema,
            IServiceCollection serviceCollection, Action<GraphBuilder> configureBuilder) where T : DbContext
        {
            serviceCollection.AddTransient<IObjectGraphRootQueryType>(s =>
            {
                var dbContext = s.GetRequiredService<T>();

                var graphBuilder = new GraphBuilder(dbContext);
                configureBuilder(graphBuilder);

                schema.RegisterTypes(graphBuilder.BuildGraphTypes().ToArray());
                var rootQuery = graphBuilder.BuildRootQueryType();

                return rootQuery;
            });


            schema.SetQueryResolver<IObjectGraphRootQueryType>((s) =>
            {
                return s.GetService<IObjectGraphRootQueryType>();
            });
            //schema.SetQueryResolver<IObjectGraphRootMutationType>((s) => s.GetService<IObjectGraphRootMutationType>());

            return schema;
        }
    }
}
