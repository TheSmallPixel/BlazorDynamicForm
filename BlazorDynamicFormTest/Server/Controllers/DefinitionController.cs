
using System.Diagnostics;
using System.Dynamic;
using System.Security.Cryptography.X509Certificates;
using BlazorDynamicForm;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlazorDynamicFormTest.Server.Controllers
{
    public  class testData
    {
        public string Provola { get; set; } = "tadaaa";
    }
    [ApiController]
    [Route("[controller]")]
    public class DefinitionController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            var def = DataAnnotationParser.ReadDataAnnotationsAsJson<TestLol>((option) =>
            {
            });
            //var data = def.CreateObject((option) =>
            //{
            //    option.InitStringsEmpty = true;
            //    option.MaxRecursiveDepth = 10;
            //});
            //var isValid = def.Validate(data);
            //dynamic dataTest = new ExpandoObject();       
            //dataTest.Provola = "bella";
            //var isValid2 = def.Validate(dataTest as ExpandoObject);
            //var dataTest2 = new testData();
            //var isValid3 = def.Validate(dataTest2);
            //var json = JsonConvert.SerializeObject(data, Formatting.None);
            return Ok(def);
        }
    }


}
