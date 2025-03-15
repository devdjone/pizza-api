using Microsoft.AspNetCore.Mvc;
using pizza_api.Services;

namespace pizza_api.Controllers
{
    


    [ApiController]
    [Route("[controller]")]
    public class AddServiceController : ControllerBase
    {
        [HttpGet(Name = "GetIndexDD")]
        public  async Task<string> Index(string campaignName)
        {
            //string campaignName = "abc";
            campaignName = campaignName.ToLower();

            var newCampaign = new KnativeServiceCreator();

            await newCampaign.CreateAsync(campaignName);
            return $"add campaign: {campaignName}";
        }



        [HttpPost(Name = "RunJob")]
        public async Task<string> Index2(string campaignId)
        {
            //string campaignName = "abc";
            campaignId = campaignId.ToLower();

            var newCampaign = new OpenShiftJobCreator();

            await newCampaign.CreateAsync(campaignId);
            return $"add campaign: {campaignId}";
        }




    }
}
