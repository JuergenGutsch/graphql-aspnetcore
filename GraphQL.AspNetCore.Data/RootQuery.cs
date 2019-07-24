using GraphQL.Types;

namespace GraphQL.AspNetCore.Data
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery(params IObjectGraphRootType[] graphTypes)
        {
            foreach (var graphType in graphTypes)
            {
                this.AddFields(graphType.Fields);
            }
        }
    }
}