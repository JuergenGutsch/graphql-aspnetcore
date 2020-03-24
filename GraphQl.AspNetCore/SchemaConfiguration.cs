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

        private Func<IServiceProvider, IObjectGraphType> _mutationResolver;
        public void SetMutationResolver<T>(Func<IServiceProvider, IObjectGraphType> resolver) where T : IObjectGraphType
        {
            _mutationResolver = resolver;
        }

        public void SetQueryType<T>() where T : IObjectGraphType
        {
            _queryType = typeof(T);
        }

        private Func<IServiceProvider, IObjectGraphType> _queryResolver;
        public void SetQueryResolver<T>(Func<IServiceProvider, IObjectGraphType> resolver) where T : IObjectGraphType
        {
            _queryResolver = resolver;
        }

        public void SetSubscriptionType<T>() where T : IObjectGraphType
        {
            _subscriptionType = typeof(T);
        }

        private Func<IServiceProvider, IObjectGraphType> _subscriptionResolver;
        public void SetSubscriptionResolver<T>(Func<IServiceProvider, IObjectGraphType> resolver) where T : IObjectGraphType
        {
            _subscriptionResolver = resolver;
        }

        public void SetFieldNameConverter<T>() where T : IFieldNameConverter
        {
            _fileNameConverterType = typeof(T);
        }


        private IGraphType[] _types;
        public void RegisterTypes(params IGraphType[] types)
        {
            _types = types;
        }

        ISchema ISchemaProvider.Create(IServiceProvider services)
        {
            var dependencyResolver = new GraphQlDependencyResolver(services);
            var schema = new Schema(dependencyResolver);

            if (_types != null && _types.Any())
            {
                schema.RegisterTypes(_types);
            }

            if (_queryResolver != null)
                schema.Query = _queryResolver(services);

            if (_mutationResolver != null)
                schema.Mutation = _mutationResolver(services);

            if (_subscriptionType != null)
                schema.Subscription = _subscriptionResolver(services);

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
