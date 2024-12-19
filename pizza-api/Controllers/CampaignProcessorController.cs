using Microsoft.AspNetCore.Mvc;

namespace pizza_api.Controllers
{
    public class CampaignProcessorController : Controller
    {
        public IActionResult Index()
        {
            // take message  
            return View();
        }
    }
}
