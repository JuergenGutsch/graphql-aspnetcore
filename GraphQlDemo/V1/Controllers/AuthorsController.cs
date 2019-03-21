using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GraphQlDemo.Models;
using GraphQlDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace GraphQlDemo.V1.Controllers
{
    [ApiController]
    [Route("api/v1/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly IBookService _bookService;

        public AuthorsController(
            IAuthorService authorService,
            IBookService bookService)
        {
            _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<Author>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetAuthors()
        {
            var result = await _authorService.GetAuthorsAsync();
            return Ok(result);
        }

        [HttpGet("{authorId:int}")]
        [ProducesResponseType(typeof(Author), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAuthorById(int authorId)
        {
            var result = await _authorService.GetAuthorByIdAsync(authorId);

            if(result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("{authorId:int}/books")]
        [ProducesResponseType(typeof(Author), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBooksByAuthorId(int authorId)
        {
            var result = await _bookService.GetBooksByAuthorIdAsync(authorId);
            return Ok(result);
        }
    }
}