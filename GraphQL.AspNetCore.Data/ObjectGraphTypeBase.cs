using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.AspNetCore.Data
{
    public class ObjectGraphTypeBase<T> : ObjectGraphType where T: class
    {
        private readonly DbContext _context;

        public ObjectGraphTypeBase(DbContext context)
        {
            _context = context;

            // get all
            FieldAsync<ListGraphType<ObjectGraphType<T>>>(
                name: "books",
                resolve: async context => await _context.Set<T>().ToListAsync()
            );



        }
    }
}
