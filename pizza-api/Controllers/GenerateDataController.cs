using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using pizza_api.Services;

namespace pizza_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenerateDataController : Controller
    {
        //[ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("create")]
        public IActionResult Index(string fileName, string campaignName, int rowsToGenerate)
        {
            var generator = new CsvFileGenerator();
            var newCampaign = new CampaignDataService();
            var data = newCampaign.GenerateFakeCampaignRecipients(1, rowsToGenerate);


            generator.GenerateCsvFile(fileName, data);
            return View();
        }
    }
}
