namespace GraphQL.AspNetCore.Data
{
    public static class GraphQLTypeBuilder
    {

        public static GraphQLTypeBuilder<T> CreateFor<T>()
        {
            return new GraphQLTypeBuilder<T>();
        }


    }
}