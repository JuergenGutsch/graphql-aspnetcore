
using GraphQL.Builders;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GraphQL.AspNetCore.Data
{
    public class GraphBuilder
    {
        private List<IBuildGraphQLType> _graphBuilders;
        private IModel _model;

        public GraphBuilder()
        {
            _graphBuilders = new List<IBuildGraphQLType>();
        }


        public GraphBuilder(IModel model = null) : this()
        {
            _model = model;
        }

     
        public GraphBuilder Define<T>(Action<GraphQLTypeBuilder<T>> configure = null)
        {
           var builder = GraphQLTypeBuilder.CreateFor<T>();
            _graphBuilders.Add(builder);

            if (_model != null)
            {
                builder.FieldsFrom(_model);
            }
            configure?.Invoke(builder);
            return this;
        }

        public IReadOnlyCollection<IGraphType> BuildGraphTypes()
        {
            return _graphBuilders.Select(b => b.Build()).ToArray();
        }


    }

    public static class GraphQLTypeBuilderExtensions
    {
        public static GraphQLTypeBuilder<T> AsRootType<T>(
            this GraphQLTypeBuilder<T> builder,
            string name)
        {
            throw new NotImplementedException();
        }

        public static GraphQLTypeBuilder<T> HasSubType<T>(
            this GraphQLTypeBuilder<T> builder,
            Expression<Func<T, object>> expression)
        {
            throw new NotImplementedException();
        }
    }
}