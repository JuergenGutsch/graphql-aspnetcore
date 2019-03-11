using GraphQlDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQlDemo.Services
{
    public interface IBookService
    {
        Task<Book> CreateBookAsync(Book book);

        Task<Book> GetBookByIsbnAsync(string isbn);

        Task<IEnumerable<Book>> GetBooksAsync();

        Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId);

        Task<IEnumerable<Book>> GetBooksByPublisherIdAsync(int publisherId);
    }
}
