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
        private readonly IAuthorRepository authorRepository;

        public AuthorService(IAuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
        }

        public async Task<Author> GetAuthorByIdAsync(int authorId)
        {
            if (authorId == default(int))
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return await this.authorRepository.GetAuthorByIdAsync(authorId);
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return await this.authorRepository.GetAuthorsAsync();
        }

        public async Task<IEnumerable<Author>> GetAuthorsByPublisherIdAsync(int publisherId)
        {
            if (publisherId == default(int))
            {
                throw new ArgumentNullException(nameof(publisherId));
            }

            return await this.authorRepository.GetAuthorsByPublisherIdAsync(publisherId);
        }
    }
}
