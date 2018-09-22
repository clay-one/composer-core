using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AspNetCoreSample
{
    public class SampleController : Controller
    {
        public SampleController(IConfiguration configuration)
        {
        }

        [Route("/some/action")]
        public IActionResult SomeAction()
        {
            return Ok("Some action is called.");
        }
    }
}