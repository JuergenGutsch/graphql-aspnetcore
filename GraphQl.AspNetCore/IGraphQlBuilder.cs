using System;

namespace GraphQl.AspNetCore
{
    public interface IGraphQlBuilder
    {
        /// <summary>
        /// Configure another schema
        /// </summary>
        /// <param name="name"></param>
        /// <param name="configure"></param>
        /// <returns></returns>
        IGraphQlBuilder AddSchema(string name, Action<SchemaConfiguration> configure);

        IGraphQlBuilder AddGraphTypes();
    }
}
