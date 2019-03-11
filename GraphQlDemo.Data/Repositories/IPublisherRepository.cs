using GraphQlDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQlDemo.Data.Repositories
{
    public interface IPublisherRepository
    {
        Task<Publisher> GetPublisherByIdAsync(int publisherId);

        Task<IEnumerable<Publisher>> GetPublishersAsync();
    }
}
