using System.Collections.Generic;
using ComposerCore;
using ComposerCore.Attributes;
using Microsoft.AspNetCore.Mvc;
using WebApiSample.Components;

namespace WebApiSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IComposer _composer;
        
        public ValuesController(IComposer composer)
        {
            // Constructor injection
            _composer = composer;
        }

        // Property injection
        [ComponentPlug]
        public IValueProvider ValueProvider { get; set; }
        
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return ValueProvider.GetValues();
        }
    }
}