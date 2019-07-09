using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GraphQL.AspNetCore.Data
{
    public class GraphQlContext
    {

        public GraphQlContext(DbContext dbContext)
                : this(dbContext.Model)
        {

        }


        public GraphQlContext(IModel model)
        {
            Model = model;
        }

        public GraphQlContext(DbContextOptions dbContextOptions)
        {
            //dbContextOptions.FindExtension
        }

        public IModel Model { get; }

        protected virtual void OnGraphCreating(GraphBuilder graphlBuilder)
        {


            






        }

        public void Build()
        {







        }














    }







}
