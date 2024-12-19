using k8s.KubeConfigModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared;
using pizza_api.Commands;
using pizza_api.Data;
using pizza_api.Repository;
using pizza_api.Services;

namespace pizza_api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CampaignController : ControllerBase
    {

        private readonly ICampaignRepository _campaignRepository;

        public CampaignController(ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateCampaignCommand cmd)
        {

            await Task.Delay(0);
            if (ModelState.IsValid)
            {
                //var newCampaign = new CampaignDataService();
                
                //var data = newCampaign.GenerateCampaignData(cmd.Name,cmd.Rows);

                //_campaignRepository.AddCampaigns(data);


                var loader = new CampaignLoader(_campaignRepository);

                loader.LoadCampaignData(cmd);


                return Ok();
            }
            else
                return BadRequest(ModelState);
        }
    }
}
