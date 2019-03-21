using GraphQL.Types;
using GraphQlDemo.Models;

namespace GraphQlDemo.GraphQl.Types
{
    public class PublisherType : ObjectGraphType<Publisher>
    {
        public PublisherType()
        {
            Field(m => m.Id);
            Field(m => m.Name);
        }
    }
}
