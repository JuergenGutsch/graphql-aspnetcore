using GraphQlDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQlDemo.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAuthorsAsync();

        Task<Author> GetAuthorByIdAsync(int authorId);

        Task<IEnumerable<Author>> GetAuthorsByPublisherIdAsync(int publisherId);
    }
}
