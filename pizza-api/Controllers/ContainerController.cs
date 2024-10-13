using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace pizza_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContainerController : ControllerBase
    {
        [HttpGet(Name = "Index")]
        public string Index()
        {
            return Dns.GetHostName();
        }
    }
}
