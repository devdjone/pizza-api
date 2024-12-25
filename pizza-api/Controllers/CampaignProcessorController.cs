using Microsoft.AspNetCore.Mvc;
using pizza_api.Commands;
using pizza_api.Models;
using pizza_api.Repository;
using pizza_api.Services;
using System.Net;

namespace pizza_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CampaignProcessorController : Controller
    {
        private readonly ICampaignRepository _campaignRepository;

        public CampaignProcessorController(ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository;
        }


        [HttpPost]
        [Route("process")]
        public async Task<IActionResult> Process([FromBody] ProcessMessageCommand cmd)
        {

            await Task.Delay(0);
            if (ModelState.IsValid)
            {

                var recipients = cmd.Recipients;
                var recList = new List<CampaignRecipient>();
                foreach (var recipient in recipients)
                {
                    var r = new CampaignRecipient();
                    r.Id = recipient.Id;
                    r.SentConfirmed = true;
                    r.ProcessedBy = Dns.GetHostName();
                    recList.Add(r);
                }

                _campaignRepository.UpdateCampaignSentConfirmFlag(recList);
                

                return Ok();
            }
            else
                return BadRequest(ModelState);
        }
    }
}
 
