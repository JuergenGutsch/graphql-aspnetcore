using Microsoft.EntityFrameworkCore;

namespace GraphQL.AspNetCore.Data
{
    public class GraphQlDbContext : DbContext
    {

        protected virtual void OnGraphCreating(GraphBuilder graphlBuilder)
        {

        }
    }

}
