using pizza_api.Models;

namespace pizza_api.Repository
{
    public interface ICampaignRepository
    {
        void AddCampaign(Campaign campaign);
        void AddCampaigns(IEnumerable<Campaign> campaigns);
        IEnumerable<Campaign> GetAllCampaigns();
        Campaign GetCampaignById(int id);
        void RemoveCampaign(int id);
    }
}