using System.Collections.Generic;
using GraphQL;

namespace GraphQl.AspNetCore
{
    internal class GraphQlParameters
    {
        public string Query { get; set; }
        public string OperationName { get; set; }
        public Dictionary<string, object> Variables { get; set; }

        public Inputs GetInputs()
        {
            return (Variables != null) ? new Inputs(Variables) : new Inputs();
        }
    }
}
