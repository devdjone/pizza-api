using Microsoft.AspNetCore.Mvc;

namespace pizza_api.Controllers
{
    public class CampaignActivatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
