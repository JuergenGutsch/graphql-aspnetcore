using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GraphQlDemo.Models;
using GraphQlDemo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraphQlDemo.V1.Controllers
{
    [ApiController]
    [Route("api/v1/publishers")]
    public class PublishersController : ControllerBase
    {
        private readonly IPublisherService publisherService;
        private readonly IAuthorService authorService;
        private readonly IBookService bookService;

        public PublishersController(
            IPublisherService publisherService,
            IAuthorService authorService,
            IBookService bookService)
        {
            this.publisherService = publisherService ?? throw new ArgumentNullException(nameof(publisherService));
            this.authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
            this.bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<Publisher>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPublishers()
        {
            var result = await this.publisherService.GetPublishersAsync();
            return this.Ok(result);
        }

        [HttpGet("{publisherId:int}")]
        [ProducesResponseType(typeof(Publisher), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetPublisherById(int publisherId)
        {
            var result = await this.publisherService.GetPublisherByIdAsync(publisherId);

            if (result == null)
            {
                return this.NotFound();
            }

            return this.Ok(result);
        }

        [HttpGet("{publisherId:int}/authors")]
        [ProducesResponseType(typeof(IEnumerable<Author>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAuthorsByPublisherId(int publisherId)
        {
            var result = await this.authorService.GetAuthorsByPublisherIdAsync(publisherId);
            return this.Ok(result);
        }

        [HttpGet("{publisherId:int}/books")]
        [ProducesResponseType(typeof(IEnumerable<Book>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBooksByPublisherId(int publisherId)
        {
            var result = await this.bookService.GetBooksByPublisherIdAsync(publisherId);
            return this.Ok(result);
        }
    }
}