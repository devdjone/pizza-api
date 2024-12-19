using NuGet.Packaging;
using pizza_api.Commands;
using pizza_api.Data;
using pizza_api.Models;
using pizza_api.Repository;

namespace pizza_api.Services
{
    public class CampaignLoader
    {
        private readonly ICampaignRepository _campaignRepository;

        
        public CampaignLoader(ICampaignRepository campaignRepository)
        {
            _campaignRepository = campaignRepository;
         
        }

        public bool LoadCampaignData(CreateCampaignCommand cmd)
        {
            var newCampaign = new CampaignDataService();

            var data = newCampaign.GenerateCampaignData(cmd.Name, cmd.Rows);

            _campaignRepository.AddCampaigns(data);
            return true;

        }

        public bool LoadCampaignData999(CreateCampaignCommand cmd)
        {
            var newCampaign = new CampaignDataService();

            var campaign = newCampaign.GenerateCampaignData(cmd.Name, 1).First();
            _campaignRepository.AddCampaign(campaign);

            // If the number of rows exceeds 1000, insert data in batches
            if (cmd.Rows > 1000)
            {
                GenerateAndInsertInBatches(newCampaign, campaign, cmd.Rows, 1000);
            }
            else
            {
                var data = newCampaign.GenerateCampaignData(cmd.Name, cmd.Rows);
                var recipeints = newCampaign.GenerateFakeCampaignRecipients(campaign.Id, cmd.Rows);
                _campaignRepository.AddCampaignRecipients(recipeints); 
            }

            return true;
        }

        //private void InsertInBatches(List<Campaign> data, int batchSize)
        //{
        //    for (int i = 0; i < data.Count; i += batchSize)
        //    {
        //        // Get the batch of data (e.g., 1000 rows at a time)
        //        var batch = data.Skip(i).Take(batchSize).ToList();
        //        _campaignRepository.AddCampaigns(batch);  // Insert this batch into the repository
        //    }
        //}

        //private void GenerateAndInsertInBatches1(CampaignDataService newCampaign, string campaignName, int totalRows, int batchSize)
        //{
        //    for (int i = 0; i < totalRows; i += batchSize)
        //    {
        //        // Generate a batch of data
        //        int rowsToGenerate = Math.Min(batchSize, totalRows - i);  // Ensure we don't generate more rows than needed
        //        var batch = newCampaign.GenerateCampaignData(campaignName, rowsToGenerate);

        //        // Insert the batch into the repository
        //        _campaignRepository.AddCampaigns(batch);
        //    }
        //}

        private void GenerateAndInsertInBatches(CampaignDataService newCampaign, Campaign campaign, int totalRows, int batchSize)
        {
            for (int i = 0; i < totalRows; i += batchSize)
            {
                // Generate a batch of data (recipients) for the existing campaign
                int rowsToGenerate = Math.Min(batchSize, totalRows - i);  // Ensure we don't generate more rows than needed
                var batchRecipients = newCampaign.GenerateFakeCampaignRecipients(campaign.Id, rowsToGenerate);

               

                // Associate the recipients with the campaign
                _campaignRepository.AddCampaignRecipients(batchRecipients);

                // Insert the batch into the repository
                
            }
        }
    }
}
