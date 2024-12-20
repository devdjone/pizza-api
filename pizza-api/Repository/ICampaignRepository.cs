using pizza_api.Models;

namespace pizza_api.Repository
{
    public interface ICampaignRepository
    {
        void AddCampaign(Campaign campaign);
        void AddCampaignRecipients(List<CampaignRecipient> recipients);

        void AddCampaigns(IEnumerable<Campaign> campaigns);
        IEnumerable<Campaign> GetAllCampaigns();
        Campaign GetCampaignById(int id);
        Task<List<CampaignRecipient>> GetRecipientsBatch(bool sentStatus, int batchSize);
        void RemoveCampaign(int id);
        void UpdateCampaignRecipients(List<CampaignRecipient> recipients);
        void UpdateCampaignRecipientsBulk(List<CampaignRecipient> recipients);
        void UpdateCampaignSentConfirmFlag(List<CampaignRecipient> recipients);
    }
}