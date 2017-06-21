using GraphQL.Types;

namespace GraphQl.AspNetCore
{
    public class GraphQlMiddlewareOptions
    {
        public ObjectGraphType RootGraphType { get; set; }
        public string GraphApiUrl { get; set; } = "/graph";
        public bool FormatOutput { get; set; } = true;
    }
}
