using GraphQL.Types;
using System.Collections.Generic;

namespace GraphQL.AspNetCore.Data
{
    public static class GraphQlTpeExtensions
    {
        /// <summary>
        /// Add multiple fields of a given ObjectGraphType
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="fieldTypes">The field types.</param>
        /// <returns></returns>
        public static IEnumerable<FieldType> AddFields(this ObjectGraphType type, 
            IEnumerable<FieldType> fieldTypes)
        {
            foreach (var fieldType in fieldTypes)
            {
                type.AddField(fieldType);
            }

            return fieldTypes;
        }
    }
}