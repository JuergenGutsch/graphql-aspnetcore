using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL.Validation;
using GraphQL.Validation.Complexity;
using Microsoft.AspNetCore.Http;

namespace GraphQl.AspNetCore
{
    public class GraphQlMiddlewareOptions
    {
        public string SchemaName { get; set; } = Defaults.DefaultSchemaName;
        public string AuthorizationPolicy { get; set; }

        public bool FormatOutput { get; set; } = true;
        public ComplexityConfiguration ComplexityConfiguration { get; set; } = new ComplexityConfiguration();
        public bool ExposeExceptions { get; set; }

        public IList<IValidationRule> ValidationRules { get; } = new List<IValidationRule>();
        public Func<HttpContext, Task<GraphQLUserContext>> BuildUserContext { get; set; }
        public bool EnableMetrics { get; set; }
    }
}
