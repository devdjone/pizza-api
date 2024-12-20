using Microsoft.AspNetCore.Mvc;
using pizza_api.Commands;
using pizza_api.Repository;
using pizza_api.Services;

namespace pizza_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CampaignActivatorController : ControllerBase
    {
        private readonly IMessageLoaderService _messageService;

        public CampaignActivatorController(IMessageLoaderService messageService)
        {
            _messageService = messageService;
        }


        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] ActivateCampaignCommand cmd)
        {

            await Task.Delay(0);
            if (ModelState.IsValid)
            {
                
                var message = _messageService.ProcessCampaignRecipientsAsync(cmd);
 
                return Ok();
            }
            else
                return BadRequest(ModelState);
        }
    }
}
