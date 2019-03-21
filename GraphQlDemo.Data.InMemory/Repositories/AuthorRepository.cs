using GraphQlDemo.Data.Repositories;
using GraphQlDemo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQlDemo.Data.InMemory.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private static List<Author> _authors;

        /// <summary>
        /// Setup sample data
        /// </summary>
        public static void Initialize()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            using (var reader = new StreamReader($"{path}/books.json"))
            {
                var json = reader.ReadToEnd();
                var books = JsonConvert.DeserializeObject<List<Book>>(json);
                _authors = books.Select(m => new Author
                {
                    Id = m.Author.Id,
                    Name = m.Author.Name,
                    Publishers = new List<int> { m.Publisher.Id }
                }).ToList();
            }
        }

        public async Task<Author> GetAuthorByIdAsync(int authorId)
        {
            return await Task.FromResult(_authors.FirstOrDefault(m => m.Id == authorId));
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return await Task.FromResult(_authors.AsEnumerable());
        }

        public async Task<IEnumerable<Author>> GetAuthorsByPublisherIdAsync(int publisherId)
        {
            return await Task.FromResult(_authors.Where(m => m.Publishers.Any(x => x == publisherId)));
        }
    }
}
