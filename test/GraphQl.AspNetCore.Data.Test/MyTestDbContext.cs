using Microsoft.EntityFrameworkCore;

namespace GraphQl.AspNetCore.Data.Test
{
    public class MyTestDbContext : DbContext
    {
        public MyTestDbContext(DbContextOptions options) : base(options)
        {
        }

        public MyTestDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<MyClass>().HasKey(e => e.Id);
            modelBuilder.Entity<MyClass>().Property(e => e.AnyProperty);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseInMemoryDatabase("InMemoryDatabase");
        }

    }
}
