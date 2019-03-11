using GraphQlDemo.Data.Repositories;
using GraphQlDemo.Models;
using GraphQlDemo.Services.Validation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GraphQlDemo.Services.Implementations
{
    public class BookService : IBookService
    {
        private readonly IBookRepository bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            this.bookRepository = bookRepository;
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            var createBookValidator = new CreateBookValidator(this);
            var validationResult = createBookValidator.Validate(book);

            if (!validationResult.IsValid)
            {
                // TODO: implement decent error handling (e.g: middleware)
                var exception = new Exception("Validation for book failed");
                foreach(var error in validationResult.Errors)
                {
                    exception.Data.Add(error.PropertyName, error.ErrorMessage);
                }
                throw exception;
            }

            var result = await this.bookRepository.CreateBookAsync(book);
            if (result == null)
            {
                throw new Exception("Creating book failed");
            }

            return result;
        }

        public async Task<Book> GetBookByIsbnAsync(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                throw new ArgumentNullException(nameof(isbn));
            }

            return await this.bookRepository.GetBookByIsbnAsync(isbn);
        }

        public async Task<IEnumerable<Book>> GetBooksAsync()
        {
            return await this.bookRepository.GetBooksAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(int authorId)
        {
            if (authorId == default(int))
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return await this.bookRepository.GetBooksByAuthorIdAsync(authorId);
        }

        public async Task<IEnumerable<Book>> GetBooksByPublisherIdAsync(int publisherId)
        {
            if (publisherId == default(int))
            {
                throw new ArgumentNullException(nameof(publisherId));
            }

            return await this.bookRepository.GetBooksByPublisherIdAsync(publisherId);
        }
    }
}
