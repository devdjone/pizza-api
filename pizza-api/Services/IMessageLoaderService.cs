
using pizza_api.Commands;

namespace pizza_api.Services
{
    public interface IMessageLoaderService
    {
        
        Task ProcessCampaignRecipientsAsync(ActivateCampaignCommand cmd);
    }
}