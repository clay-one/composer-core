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
        public ValuesController(IComposer composer)
        {
            ValueProvider = composer.GetComponent<IValueProvider>();
        }

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