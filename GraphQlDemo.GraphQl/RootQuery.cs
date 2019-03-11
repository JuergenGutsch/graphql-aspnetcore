using GraphQL.Types;
using GraphQlDemo.GraphQl.Extensions;
using GraphQlDemo.GraphQl.RootTypes;

namespace GraphQlDemo.GraphQl
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery(BookRootTypes bookRootTypes)
        {
            this.AddFields(bookRootTypes.Fields);
        }
    }
}
