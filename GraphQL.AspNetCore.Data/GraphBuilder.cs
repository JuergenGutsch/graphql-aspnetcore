using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphQL.AspNetCore.Data
{
    public class GraphBuilder
    {
        private readonly List<IBuildGraphQLType> _graphBuilders;
        private readonly IModel _model;
        private readonly DbContext _dbContext;

        public GraphBuilder()
        {
            _graphBuilders = new List<IBuildGraphQLType>();
        }

        public GraphBuilder(DbContext dbContext = null)
            : this()
        {
            _model = dbContext.Model;
            _dbContext = dbContext;
        }

        public GraphBuilder Define<T>(Action<GraphQLTypeBuilder<T>> configure = null) where T : class
        {
            var typeBuilder = GraphQLTypeBuilder.CreateFor<T>();
            _graphBuilders.Add(typeBuilder);

            var rootTypeBuilder = GraphQLRootTypeBuilder.CreateFor<T>(_dbContext);
            _graphBuilders.Add(rootTypeBuilder);

            if (_model != null)
            {
                typeBuilder.FieldsFrom(_model);
            }
            configure?.Invoke(typeBuilder);
            return this;
        }

        public IReadOnlyCollection<IGraphType> BuildGraphTypes()
        {
            var types = _graphBuilders.Select(b => b.Build());
            return types.ToArray();
        }
    }
}