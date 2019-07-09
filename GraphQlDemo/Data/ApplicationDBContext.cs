using GraphQL.AspNetCore.Data;
using GraphQlDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQlDemo.Data
{
    public class ApplicationDBContext : GraphQlDbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(b => b.Author)
                .IsRequired();
        }

        protected override void OnGraphCreating(GraphBuilder graphlBuilder)
        {
            graphlBuilder.Entity<Book>()
                .AsRootType(name: "books")
                .HasSubType(x => x.Author)
                .HasSubType(x => x.Publisher);
            graphlBuilder.Entity<Publisher>()
                .AsRootType(name: "publishers");
            graphlBuilder.Entity<Author>()
                .AsRootType(name: "authors");

        }
    }



  

}
