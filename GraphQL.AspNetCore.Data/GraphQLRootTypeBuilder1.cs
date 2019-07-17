using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.AspNetCore.Data
{
    public class GraphQLRootTypeBuilder<T> : IBuildGraphQLType where T : class
    {
        private readonly DbContext _dbContext;

        public GraphQLRootTypeBuilder(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbContext DbContext { get; }

        public IGraphType Build()
        {
            var o = new ObjectGraphTypeBase<T>(_dbContext);

            return o;
        }
    }

}