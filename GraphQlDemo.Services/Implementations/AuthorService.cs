using GraphQlDemo.Data.Repositories;
using GraphQlDemo.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GraphQlDemo.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
        }

        public async Task<Author> GetAuthorByIdAsync(int authorId)
        {
            if (authorId == default(int))
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return await _authorRepository.GetAuthorByIdAsync(authorId);
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return await _authorRepository.GetAuthorsAsync();
        }
    }
}
