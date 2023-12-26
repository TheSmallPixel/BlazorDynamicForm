
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlazorDynamicFormTest.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DefinitionController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            var def = DataAnnotationParser.ReadDataAnnotations<TestLol>();
            string json = JsonConvert.SerializeObject(def, Formatting.Indented, new JsonSerializerSettings()
            {
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                TypeNameHandling = TypeNameHandling.Auto,
                NullValueHandling = NullValueHandling.Ignore
            });
            return Ok(json);
        }
    }
}
