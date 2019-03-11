using GraphQlDemo.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQlDemo.Data.Repositories
{
    public interface IBookRepository
    {
        Task<Book> GetBookByIsbn(string isbn);

        Task<IEnumerable<Book>> GetBooksAsync();

        Task<Book> CreateBookAsync(Book book);
    }
}
