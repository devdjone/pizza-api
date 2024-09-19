using Microsoft.AspNetCore.Mvc;

namespace pizza_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {
        [HttpGet(Name = "GetIndex")]
        public string Index()
        {
            return "hellow 5";
        }
    }
}
