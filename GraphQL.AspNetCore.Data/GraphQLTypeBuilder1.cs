using GraphQL.Types;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GraphQL.AspNetCore.Data
{
    public class GraphQLTypeBuilder<T> : IBuildGraphQLType
    {
        private Dictionary<string, EventStreamFieldType> _fields = new Dictionary<string, EventStreamFieldType>();

        public GraphQLTypeBuilder<T> FieldsFrom(IModel model)
        {
            var entityType = model.GetEntityTypes().SingleOrDefault(et => et.ClrType == typeof(T));

            if (entityType != null)
            {
                foreach (var item in entityType.GetProperties())
                {
                    GetOrCreate(item.Name);
                }
            }
            return this;
        }

        public GraphQLTypeBuilder<T> Field<TV>(Expression<Func<T, TV>> expression, Action<FieldBuilder> configure)
        {
            var field = GetOrCreate(expression.NameOf());
            var fieldBuilder = new FieldBuilder(field);
            configure(fieldBuilder);
            return this;
        }

        IGraphType IBuildGraphQLType.Build()
        {
            return Build();
        }

        public ObjectGraphType<T> Build()
        {
            var o = new ObjectGraphType<T>();
            foreach (var item in _fields)
            {
                o.AddField(item.Value);
            }
            return o;
        }

        private EventStreamFieldType GetOrCreate(string name)
        {
            return _fields.ContainsKey(name) ? _fields[name] : (_fields[name] = new EventStreamFieldType()
            {
                Name = name,
                Type = typeof(ObjectGraphType<T>)
            });
        }
    }

}