using GraphQlDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQlDemo.Data.Repositories
{
    public interface IBookRepository
    {
        Task<Book> GetBookByIsbnAsync(string isbn);

        Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId);

        Task<IEnumerable<Book>> GetBooksByPublisherIdAsync(int publisherId);

        Task<IEnumerable<Book>> GetBooksAsync();

        Task<Book> CreateBookAsync(Book book);
    }
}
