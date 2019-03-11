using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using GraphQlDemo.Models;
using GraphQlDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace GraphQlDemo.Controllers
{
    [ApiController]
    [Route("api/v1/books")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService bookService;

        public BooksController(IBookService bookService)
        {
            this.bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<Book>), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> GetBooks()
        {
            var result = await this.bookService.GetBooksAsync();
            return this.Ok(result);
        }

        [HttpGet("{isbn}")]
        [ProducesResponseType(typeof(Book), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBookByIsbn(string isbn)
        {
            var result = await this.bookService.GetBookByIsbnAsync(isbn);

            if(result == null)
            {
                return this.NotFound();
            }

            return this.Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Book), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Create([FromBody] Book book)
        {
            var result = await this.bookService.CreateBookAsync(book);
            return this.Created(Url.Action("GetBookByIsbn", new { isbn = result.Isbn }), result);
        }
    }
}