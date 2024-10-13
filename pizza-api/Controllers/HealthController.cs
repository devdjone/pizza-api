using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace pizza_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet()]
        public string Index()
        {
            return "Up";
        }
    }
}
