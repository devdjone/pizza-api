using Microsoft.AspNetCore.Mvc;
using pizza_api.Services;

namespace pizza_api.Controllers
{
    


    [ApiController]
    [Route("[controller]")]
    public class AddServiceController : ControllerBase
    {
        [HttpGet(Name = "GetIndexDD")]
        public  async Task<string> Index()
        {
            string campaignName = "B";
            var newCampaign = new CreateKnativeService();

            await newCampaign.CreateAsync(campaignName);
            return $"add campaign: {campaignName}";
        }
    }
}
