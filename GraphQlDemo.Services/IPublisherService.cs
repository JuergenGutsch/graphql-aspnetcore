using GraphQlDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQlDemo.Services
{
    public interface IPublisherService
    {
        Task<IEnumerable<Publisher>> GetPublishersAsync();

        Task<Publisher> GetPublisherByIdAsync(int publisherId);
    }
}
