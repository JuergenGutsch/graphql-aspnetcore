using GraphQL.Types;
using GraphQlDemo.Models;

namespace GraphQlDemo.GraphQl.Types
{
    public class BookType : ObjectGraphType<Book>
    {
        public BookType()
        {
            this.Field(m => m.Isbn);
            this.Field(m => m.Title);
            this.Field(m => m.SubTitle);
            this.Field<AuthorType>("Author");
            this.Field<PublisherType>("Publisher");
            this.Field(m => m.Published);
            this.Field(m => m.Pages);
            this.Field(m => m.Description);
        }
    }
}
