using Microsoft.AspNetCore.Mvc;

namespace GraphQlDemo.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        /// GET /
        /// <summary>
        /// The index action.
        /// </summary>
        /// <returns>The action result.</returns>
        public IActionResult Index()
        {
            // TODO: Enhancement: add swagger for REST documentation

            return this.Ok("up-and-running");
        }
    }
}