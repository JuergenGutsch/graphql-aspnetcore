using System.Collections.Generic;
using System.Linq;
using GraphQL;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GraphQl.AspNetCore
{
    internal class GraphQlParameters
    {
        public string Query { get; set; }
        public string OperationName { get; set; }
        public Dictionary<string, object> Variables { get; set; }

        public Inputs GetInputs()
        {
            Dictionary<string, object> sanitizedVariables = null;

            if (Variables != null && Variables.Any())
            {
                sanitizedVariables = new Dictionary<string, object>();

                foreach (var key in Variables.Keys)
                {
                    var value = Variables[key];

                    if (value is JObject)
                    {
                        // fix the nesting ( {{ }} )
                        var serialized = JsonConvert.SerializeObject(value);
                        var deserialized = JsonConvert.DeserializeObject<Dictionary<string, object>>(serialized);
                        sanitizedVariables.Add(key, deserialized);
                    }
                    else
                    {
                        sanitizedVariables.Add(key, value);
                    }
                }

                return new Inputs(sanitizedVariables);
            }
            else
            {
                return (Variables != null) ? new Inputs(Variables) : new Inputs();
            }
        }
    }
}
