using Microsoft.AspNetCore.Mvc;
using pizza_api.Commands;
using pizza_api.Services;

namespace pizza_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CampaignProcessorController : Controller
    {
        [HttpPost]
        [Route("process")]
        public async Task<IActionResult> Process([FromBody] ProcessMessageCommand cmd)
        {

            await Task.Delay(0);
            if (ModelState.IsValid)
            {

                var recipients = cmd.Recipients;
                //var newCampaign = new CampaignDataService();

                //var data = newCampaign.GenerateCampaignData(cmd.Name,cmd.Rows);

                //_campaignRepository.AddCampaigns(data);


                //var loader = new CampaignLoader(_campaignRepository);

                //loader.LoadCampaignData(cmd);


                return Ok();
            }
            else
                return BadRequest(ModelState);
        }
    }
}
 
