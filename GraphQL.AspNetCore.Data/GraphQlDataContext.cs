using Microsoft.EntityFrameworkCore;

namespace GraphQL.AspNetCore.Data
{
    public abstract class GraphQlDbContext : DbContext
    {

        protected virtual void OnGraphCreating(GraphBuilder graphlBuilder)
        {

        }
    }
}
