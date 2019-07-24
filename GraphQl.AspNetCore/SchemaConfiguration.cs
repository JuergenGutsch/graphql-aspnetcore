using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Conversion;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQl.AspNetCore
{
    public class SchemaConfiguration : ISchemaProvider
    {
        public string Name { get; }

        private Type _queryType;
        private IObjectGraphType _queryInstance;
        private Type _mutationType;
        private Type _subscriptionType;
        private Type _fileNameConverterType;

        public SchemaConfiguration(string name)
        {
            Name = name;
        }

        // TODO: Add Directives

        public void SetMutationType<T>() where T : IObjectGraphType
        {
            _mutationType = typeof(T);
        }

        public void SetQueryType<T>() where T : IObjectGraphType
        {
            _queryType = typeof(T);
        }

        public void SetQueryInstance<T>(T instance) where T : IObjectGraphType
        {
            _queryInstance = instance;
        }


        private Func<IServiceProvider, IObjectGraphType> _queryResolver;
        public void SetResolver<T>(Func<IServiceProvider, IObjectGraphType> resolver) where T : IObjectGraphType
        {
            _queryResolver = resolver;
        }

        public void SetSubscriptionType<T>() where T : IObjectGraphType
        {
            _subscriptionType = typeof(T);
        }

        public void SetFieldNameConverter<T>() where T : IFieldNameConverter
        {
            _fileNameConverterType = typeof(T);
        }

        ISchema ISchemaProvider.Create(IServiceProvider services)
        {
            var dependencyResolver = new GraphQlDependencyResolver(services);
            var schema = new Schema(dependencyResolver);

            //if (_queryInstance == null && _queryType != null)
            //{
            //    schema.Query = (IObjectGraphType)services.GetRequiredService(_queryType);
            //}
            //else if (_queryInstance != null)
            //{
            //    schema.Query = _queryInstance;
            //}

            if (_queryResolver != null)
                schema.Query = _queryResolver(services);

            if (_mutationType != null)
                schema.Mutation = (IObjectGraphType)services.GetRequiredService(_mutationType);

            if (_subscriptionType != null)
                schema.Subscription = (IObjectGraphType)services.GetRequiredService(_subscriptionType);

            if (_fileNameConverterType != null)
                schema.FieldNameConverter = (IFieldNameConverter)services.GetRequiredService(_fileNameConverterType);

            return schema;
        }

        public static ISchemaProvider GetSchemaProvider(string name, IServiceProvider services)
        {
            var providers = services.GetService<IEnumerable<ISchemaProvider>>()
                .Where(x => x.Name == name)
                .ToArray();

            if (providers.Length == 0)
            {
                if (name == null)
                {
                    throw new InvalidOperationException("No default schema registered!");
                }
                else
                {
                    throw new InvalidOperationException($"No schema found registered with name '{name}'");
                }
            }

            if (providers.Length > 1)
            {
                throw new InvalidOperationException($"Multiple schemas registered with the same name: '{name}'");
            }

            return providers[0];
        }
    }
}
