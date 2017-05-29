using GraphQL.Types;
using GraphQlDemo.Models;

namespace GraphQlDemo.Middlewares.GraphQlTypes
{
    public class PublisherType : ObjectGraphType<Publisher>
    {
        public PublisherType()
        {
            Field(x => x.Id).Description("The id of the publisher.");
            Field(x => x.Name).Description("The name of the publisher.");
            Field<ListGraphType<BookType>>("books",
                resolve: context => new Book[] { });
            Field<ListGraphType<AuthorType>>("authors",
                resolve: context => new Author[] { });
        }
    }
}
