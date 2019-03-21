using GraphQL.Types;
using GraphQlDemo.Models;

namespace GraphQlDemo.GraphQl.Types
{
    public class BookType : ObjectGraphType<Book>
    {
        public BookType()
        {
            Field(m => m.Isbn);
            Field(m => m.Title);
            Field(m => m.SubTitle);
            Field<AuthorType>("Author");
            Field<PublisherType>("Publisher");
            Field(m => m.Published);
            Field(m => m.Pages);
            Field(m => m.Description);
        }
    }
}
