namespace pizza_api.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using pizza_api.Repository;
    using pizza_api.Models;
    using pizza_api.Commands;

    public class MessageLoaderService : IMessageLoaderService
    {
         
        private readonly HttpClient _httpClient; // HttpClient to make API calls
        private readonly ICampaignRepository _campaignRepository;

        public MessageLoaderService( HttpClient httpClient,
                                    ICampaignRepository campaignRepository)
        {
            
            _httpClient = httpClient;
            _campaignRepository = campaignRepository;
        }

        public async Task ProcessCampaignRecipientsAsync(ActivateCampaignCommand cmd)
        {
            bool hasMoreRecords = true;
            int batchSize = 100;

            while (hasMoreRecords)
            {
                // Fetch up to 1000 records with flag "Processed" = false
                //var recipients = await _dbContext.Set<CampaignRecipient>()
                //    .Where(r => r.Sent == false)
                //    .Take(batchSize)
                //    .ToListAsync();

                var recipients = await _campaignRepository.GetRecipientsBatch(false, batchSize);

                if (recipients.Any())
                {
                    // Post the records to the broker API
                    var response = await PostToBrokerApiAsync(recipients);

                    if (response.IsSuccessStatusCode)
                    {
                        // Once posted successfully, update the 'Processed' flag in the database for the fetched records
                        foreach (var recipient in recipients)
                        {
                            recipient.Sent = true;
                        }

                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        // Handle failed API call, maybe log or retry
                        Console.WriteLine($"Failed to post to broker API. Status: {response.StatusCode}");
                        break;
                    }
                }
                else
                {
                    hasMoreRecords = false; // No more unprocessed records
                }
            }
        }

        private async Task<HttpResponseMessage> PostToBrokerApiAsync(List<CampaignRecipient> recipients)
        {
            string _brokerApiUrl = "http://localhost/CampaignProcessor";
            // Assuming you want to send the recipients as JSON to the broker API
            var content = new StringContent(
                Newtonsoft.Json.JsonConvert.SerializeObject(recipients),
                System.Text.Encoding.UTF8,
                "application/json");

            // Send POST request to the broker API
            return await _httpClient.PostAsync(_brokerApiUrl, content);
        }
    }

    

}
