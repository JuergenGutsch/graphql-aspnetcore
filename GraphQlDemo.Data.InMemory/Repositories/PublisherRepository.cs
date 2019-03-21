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
    public class PublisherRepository : IPublisherRepository
    {
        private static List<Publisher> _publishers;

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
                _publishers = books.Select(m => new Publisher
                {
                    Id = m.Publisher.Id,
                    Name = m.Publisher.Name
                }).ToList();
            }
        }

        public async Task<Publisher> GetPublisherByIdAsync(int publisherId)
        {
            return await Task.FromResult(_publishers.FirstOrDefault(m => m.Id == publisherId));
        }

        public async Task<IEnumerable<Publisher>> GetPublishersAsync()
        {
            return await Task.FromResult(_publishers.AsEnumerable());
        }
    }
}
