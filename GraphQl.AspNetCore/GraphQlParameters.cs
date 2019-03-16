using Newtonsoft.Json.Linq;

namespace GraphQl.AspNetCore
{
    internal class GraphQlParameters
    {
        public string Query { get; set; }

        public string OperationName { get; set; }

        public JObject Variables { get; set; }
    }
}