using Microsoft.EntityFrameworkCore;
using System;

namespace GraphQL.AspNetCore.Data
{
    public class GraphQLRootTypeBuilder
    {
        public static GraphQLRootTypeBuilder<T> CreateFor<T>(DbContext dbContext) where T : class
        {
            return new GraphQLRootTypeBuilder<T>(dbContext);
        }
    }
}