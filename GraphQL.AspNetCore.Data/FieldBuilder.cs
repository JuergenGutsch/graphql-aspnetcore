using GraphQL.Types;
using System;

namespace GraphQL.AspNetCore.Data
{
    public class FieldBuilder
    {
        private readonly EventStreamFieldType _field;

        public FieldBuilder(EventStreamFieldType field)
        {
            _field = field ?? throw new ArgumentNullException(nameof(field));
        }


        public FieldBuilder Description(string description)
        {
            _field.Description = description;
            return this;
        }

        public FieldBuilder DefaultValue(object obj)
        {
            _field.DefaultValue = obj;
            return this;
        }



    }

}