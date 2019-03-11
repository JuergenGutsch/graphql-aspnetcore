using GraphQlDemo.Data.Repositories;
using GraphQlDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GraphQlDemo.Services.Implementations
{
    public class PublisherService : IPublisherService
    {
        private readonly IPublisherRepository publisherRepository;

        public PublisherService(IPublisherRepository publisherRepository)
        {
            this.publisherRepository = publisherRepository ?? throw new ArgumentNullException(nameof(publisherRepository));
        }

        public async Task<Publisher> GetPublisherByIdAsync(int publisherId)
        {
            if (publisherId == default(int))
            {
                throw new ArgumentNullException(nameof(publisherId));
            }

            return await this.publisherRepository.GetPublisherByIdAsync(publisherId);
        }

        public async Task<IEnumerable<Publisher>> GetPublishersAsync()
        {
            return await this.publisherRepository.GetPublishersAsync();
        }
    }
}
