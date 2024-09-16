using Microsoft.AspNetCore.Mvc;

namespace pizza_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet(Name = "home")]
        public string Index()
        {
            return "hellow 3";
        }
    }
}
