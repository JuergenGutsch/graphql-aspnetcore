using GraphQL.Types;
using GraphQlDemo.Models;

namespace GraphQlDemo.GraphQl.Types
{
    public class AuthorType : ObjectGraphType<Author>
    {
        public AuthorType()
        {
            Field(m => m.Id);
            Field(m => m.Name);
        }
    }
}
