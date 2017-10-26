namespace GraphQL.AspNetCore.Graphiql
{
    public class GraphiqlMiddlewareOptions
    {
        public string GraphiqlPath { get; set; } = "/graphiql";
        public string GraphQlEndpoint { get; set; } = "/graph";
    }
}
