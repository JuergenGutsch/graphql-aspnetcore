using System;
using GraphQL.Execution;

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

        /// <summary>
        /// Adds a <see cref="IDocumentExecutionListener"/>.
        /// </summary>
        /// <typeparam name="T">The listener to add.</typeparam>
        /// <returns></returns>
        IGraphQlBuilder AddDocumentExecutionListener<T>()
            where T : class, IDocumentExecutionListener;

        /// <summary>
        /// Adds Data Loader support.
        /// </summary>
        /// <returns></returns>
        IGraphQlBuilder AddDataLoader();
        
        IGraphQlBuilder AddGraphTypes();
    }
}
