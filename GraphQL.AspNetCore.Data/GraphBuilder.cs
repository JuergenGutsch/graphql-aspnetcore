
using System;
using System.Linq.Expressions;

namespace GraphQL.AspNetCore.Data
{
    public class GraphBuilder
    {
        public GraphQLTypeBuilder<T> Entity<T>()
        {
            throw new NotImplementedException();
        }
    }

    public class GraphQLTypeBuilder<T>
    {
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