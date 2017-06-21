using System.Linq;
using System.Collections.Generic;
using GenFu;
using System;
using GraphQlDemo.Query.Data;
using GraphQlDemo.Query.Models;

namespace GraphQlDemo.Data
{
    public class BookRepository : IBookRepository
    {
        private IEnumerable<Book> _books = new List<Book>();
        private IEnumerable<Author> _authors = new List<Author>();
        private IEnumerable<Publisher> _publisher = new List<Publisher>();

        public BookRepository()
        {
            GenFu.GenFu.Configure<Author>()
                .Fill(_ => _.Name)
                .AsLastName()
                .Fill(_=>_.Birthdate)
                .AsPastDate();
            _authors = A.ListOf<Author>(40);
            GenFu.GenFu.Configure<Publisher>()
                .Fill(_ => _.Name)
                .AsMusicArtistName();
            _publisher = A.ListOf<Publisher>(10);

            GenFu.GenFu.Configure<Book>()
               .Fill(p => p.Isbn)
               .AsISBN()
               .Fill(p => p.Name)
               .AsLoremIpsumWords(5)
               .Fill(p => p.Author)
               .WithRandom(_authors)
               .Fill(_ => _.Publisher)
               .WithRandom(_publisher);


            _books = A.ListOf<Book>(100);
        }

        public IEnumerable<Author> AllAuthors()
        {
            return _authors;
        }

        public IEnumerable<Book> AllBooks()
        {
            return _books;
        }

        public IEnumerable<Publisher> AllPublishers()
        {
            return _publisher;
        }

        public Author AuthorById(int id)
        {
            return _authors.First(_ => _.Id == id);
        }

        public Book BookByIsbn(string isbn)
        {
            return _books.First(_ => _.Isbn == isbn);
        }

        public Publisher PublisherById(int id)
        {
            return _publisher.First(_ => _.Id == id);
        }
    }

    public static class StringFillerExtensions
    {
        
        public static GenFuConfigurator<T> AsISBN<T>(this GenFuStringConfigurator<T> configurator) where T : new()
        {
            var filler = new CustomFiller<string>(configurator.PropertyInfo.Name, typeof(T), () =>
            {
                return MakeIsbn();
            });
            configurator.Maggie.RegisterFiller(filler);
            return configurator;
        }
        public static string MakeIsbn()
        {
            // 978-1-933988-27-6
            var a = A.Random.Next(100, 999);
            var b = A.Random.Next(1, 9);
            var c = A.Random.Next(100000, 999999);
            var d = A.Random.Next(10, 99);
            var e = A.Random.Next(1, 9);

            return $"{a}-{b}-{c}-{d}-{e}";
        }
    }
}
