using GraphQL.Types;
using GraphQlDemo.GraphQl.Extensions;
using GraphQlDemo.GraphQl.RootTypes;

namespace GraphQlDemo.GraphQl
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery(
            BookRootTypes bookRootTypes,
            AuthorRootTypes authorRootTypes,
            PublisherRootTypes publisherRootTypes)
        {
            this.AddFields(bookRootTypes.Fields);
            this.AddFields(authorRootTypes.Fields);
            this.AddFields(publisherRootTypes.Fields);
        }
    }
}
