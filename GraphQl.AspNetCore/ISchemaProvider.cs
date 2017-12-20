using System;
using GraphQL.Types;

namespace GraphQl.AspNetCore
{
    public interface ISchemaProvider
    {
        string Name { get; }

        ISchema Create(IServiceProvider services);
    }
}