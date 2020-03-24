using GraphQL.Types;

namespace GraphQL.AspNetCore.Data
{
    public class RootQuery : ObjectGraphType, IObjectGraphRootQueryType
    {
        public RootQuery(params IObjectGraphRootType[] graphTypes)
        {
            foreach (var graphType in graphTypes)
            {
                this.AddFields(graphType.Fields);
            }
        }
    }

    public interface IObjectGraphRootQueryType : IObjectGraphType
    {

    }
}