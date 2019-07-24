using System.Collections.Generic;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.AspNetCore.Data
{
    public class ObjectGraphRootType<T> : ObjectGraphType, IObjectGraphRootType where T : class
    {
        private readonly DbContext _context;

        public ObjectGraphRootType(DbContext context)
        {
            _context = context;

            // get all
            FieldAsync<ListGraphType<ObjectGraphType<T>>>(
                name: typeof(T).Name.ToLower(),
                resolve: async context => await _context.Set<T>().ToListAsync()
            );



        }
    }

    public interface IObjectGraphRootType : IComplexGraphType
    {
    }
}
