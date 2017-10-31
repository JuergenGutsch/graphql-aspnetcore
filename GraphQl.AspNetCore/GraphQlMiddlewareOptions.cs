using GraphQL.Validation.Complexity;

namespace GraphQl.AspNetCore
{
    public class GraphQlMiddlewareOptions
    {
        public string SchemaName { get; set; }
        public string AuthorizationPolicy { get; set; }

        public bool FormatOutput { get; set; } = true;
        public ComplexityConfiguration ComplexityConfiguration { get; set; } = new ComplexityConfiguration();
        public bool ExposeExceptions { get; set; }
    }
}
