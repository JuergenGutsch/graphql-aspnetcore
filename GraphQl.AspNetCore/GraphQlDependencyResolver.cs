using System;
using GraphQL;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQl.AspNetCore
{
    internal class GraphQlDependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _services;

        public GraphQlDependencyResolver(IServiceProvider services)
        {
            _services = services;
        }

        public T Resolve<T>()
        {
            return _services.GetService<T>();
        }

        public object Resolve(Type type)
        {
            return _services.GetService(type);
        }
    }
}
