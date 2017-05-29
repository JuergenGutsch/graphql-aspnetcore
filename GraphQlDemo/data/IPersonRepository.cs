using GraphQlDemo.Models;
using System.Collections.Generic;

namespace GraphQlDemo.Data
{
    public interface IBookRepository
    {
        Book BookByIsbn(string isbn);
        IEnumerable<Book> AllBooks();

        Author AuthorById(int id);
        IEnumerable<Author> AllAuthors();

        Publisher PublisherById(int id);
        IEnumerable<Publisher> AllPublishers();
    }
}