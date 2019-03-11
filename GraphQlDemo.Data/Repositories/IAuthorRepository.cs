using GraphQlDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GraphQlDemo.Data.Repositories
{
    public interface IAuthorRepository
    {
        Task<Author> GetAuthorByIdAsync(int authorId);

        Task<IEnumerable<Author>> GetAuthorsAsync();

        Task<IEnumerable<Author>> GetAuthorsByPublisherIdAsync(int publisherId);
    }
}
