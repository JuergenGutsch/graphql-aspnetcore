
using GraphQlDemo.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GraphQlDemo.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configure entities
            modelBuilder.Entity<Book>()
               .HasOne(x => x.Author)
               .WithMany(x => x.Books);

            modelBuilder.Entity<Book>()
               .HasOne(x => x.Publisher)
               .WithMany(x => x.Books);

            // seeding
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            using (var reader = new StreamReader($"{path}/books.json"))
            {
                var json = reader.ReadToEnd();
                var books = JsonConvert.DeserializeObject<List<Book>>(json);

                var booksToSeed = books.Select(x =>
                {
                    x.AuthorId = x.Author.Id;
                    x.Author = null;
                    x.PublisherId = x.Publisher.Id;
                    x.Publisher = null;
                    return x;
                });
                //var res = JsonConvert.SerializeObject(booksToSeed);
                //Console.Write(res);

                var authorsToSeed = books.Select(x => new Author
                {
                    Id = x.Author.Id,
                    Name = x.Author.Name
                }).Distinct(new AuthorEqualityComparer()).ToList();
                var publishersToSeed = books.Select(x => new Publisher
                {
                    Id = x.Publisher.Id,
                    Name = x.Publisher.Name
                }).Distinct(new PublisherEqualityComparer()).ToList();
                               
                modelBuilder.Entity<Author>().HasData(authorsToSeed);
                modelBuilder.Entity<Publisher>().HasData(publishersToSeed);
                modelBuilder.Entity<Book>().HasData(booksToSeed);
            }
        }

        internal class AuthorEqualityComparer : IEqualityComparer<Author>
        {
            public bool Equals(Author x, Author y)
            {
               return x.Id == y.Id;
            }

            public int GetHashCode(Author obj)
            {
                return obj.Id.GetHashCode();
            }
        }

        internal class PublisherEqualityComparer : IEqualityComparer<Publisher>
        {
            public bool Equals(Publisher x, Publisher y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(Publisher obj)
            {
                return obj.Id.GetHashCode();
            }
        }
    }
}
