using GraphQL.Types;
using GraphQlDemo.Models;

namespace GraphQlDemo.GraphQl.Types
{
    public class AuthorType : ObjectGraphType<Author>
    {
        public AuthorType()
        {
            this.Field(m => m.Id);
            this.Field(m => m.Name);
        }
    }
}
