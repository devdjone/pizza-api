using Microsoft.AspNetCore.Mvc;

namespace pizza_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet(Name = "GetHome")]
        public string Index()
        {
            return "hellow 4";
        }
    }
}
