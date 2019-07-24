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
    [Route("api/v1/publishers")]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherService _publisherService;
        private readonly IAuthorService _authorService;
        private readonly IBookService _bookService;

        public PublishersController(
            IPublisherService publisherService,
            IAuthorService authorService,
            IBookService bookService)
        {
            _publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
            _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
            _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<Publisher>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPublishers()
        {
            var result = await _publisherService.GetPublishersAsync();
            return Ok(result);
        }

        [HttpGet("{publisherId:int}")]
        [ProducesResponseType(typeof(Publisher), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPublisherById(int publisherId)
        {
            var result = await _publisherService.GetPublisherByIdAsync(publisherId);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("{publisherId:int}/books")]
        [ProducesResponseType(typeof(IEnumerable<Book>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBooksByPublisherId(int publisherId)
        {
            var result = await _bookService.GetBooksByPublisherIdAsync(publisherId);
            return Ok(result);
        }
    }
}