using GraphQL.AspNetCore.Data;
using GraphQL.Types;
using GraphQlDemo.GraphQl.Extensions;
using GraphQlDemo.Models;

namespace GraphQlDemo.GraphQl
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery(
            ObjectGraphRootType<Book> bookRootTypes,
            ObjectGraphRootType<Author> authorRootTypes,
            ObjectGraphRootType<Publisher> publisherRootTypes)
        {
            //this.AddFields(bookRootTypes.Fields);
            //this.AddFields(authorRootTypes.Fields);
            //this.AddFields(publisherRootTypes.Fields);
        }
    }
}
