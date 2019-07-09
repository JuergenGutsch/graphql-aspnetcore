
using System;
using System.Linq.Expressions;
using GraphQL.Types;

namespace GraphQL.AspNetCore.Data
{
    public static class GraphQLTypeBuilderExtensions
    {
        public static GraphQLTypeBuilder<T> AsRootType<T>(
            this GraphQLTypeBuilder<T> builder,
            string name)
        {
            var type = Activator.CreateInstance<ObjectGraphType>();
            


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