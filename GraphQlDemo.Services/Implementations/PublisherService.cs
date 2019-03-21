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
        private readonly IPublisherRepository _publisherRepository;

        public PublisherService(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository ?? throw new ArgumentNullException(nameof(publisherRepository));
        }

        public async Task<Publisher> GetPublisherByIdAsync(int publisherId)
        {
            if (publisherId == default(int))
            {
                throw new ArgumentNullException(nameof(publisherId));
            }

            return await _publisherRepository.GetPublisherByIdAsync(publisherId);
        }

        public async Task<IEnumerable<Publisher>> GetPublishersAsync()
        {
            return await _publisherRepository.GetPublishersAsync();
        }
    }
}
