using GraphQL.Types;

namespace GraphQL.AspNetCore.Data
{
    public interface IBuildGraphQLType
    {
        IGraphType Build();
    }

}