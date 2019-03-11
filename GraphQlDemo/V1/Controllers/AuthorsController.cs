using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GraphQlDemo.Data.Repositories;
using GraphQlDemo.Models;
using GraphQlDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraphQlDemo.V1.Controllers
{
    [ApiController]
    [Route("api/v1/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService authorService;
        private readonly IBookService bookService;

        public AuthorsController(
            IAuthorService authorService,
            IBookService bookService)
        {
            this.authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
            this.bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<Author>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAuthors()
        {
            var result = await this.authorService.GetAuthorsAsync();
            return this.Ok(result);
        }

        [HttpGet("{authorId:int}")]
        [ProducesResponseType(typeof(Author), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAuthorById(int authorId)
        {
            var result = await this.authorService.GetAuthorByIdAsync(authorId);

            if(result == null)
            {
                return this.NotFound();
            }

            return this.Ok(result);
        }

        [HttpGet("{authorId:int}/books")]
        [ProducesResponseType(typeof(Author), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBooksByAuthorId(int authorId)
        {
            var result = await this.bookService.GetBooksByAuthorIdAsync(authorId);
            return this.Ok(result);
        }
    }
}