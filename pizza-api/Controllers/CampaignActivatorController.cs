using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pizza_api.Commands;
using pizza_api.Data;
using pizza_api.DTO;
using pizza_api.Models;
using System.Text;
using System.Text.Json;

namespace pizza_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CampaignActivatorController : ControllerBase
    {
        private readonly pizza_apiContext _context;
        private readonly HttpClient _httpClient;

        public CampaignActivatorController(pizza_apiContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }


        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] ActivateCampaignCommand cmd)
        {

            await Task.Delay(0);
            if (ModelState.IsValid)
            {
                 await ProcessCampaignRecipients(cmd);
                return Ok();
            }
            else
                return BadRequest(ModelState);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task ProcessCampaignRecipients(ActivateCampaignCommand cmd)
        {
            bool hasMoreRecords = true;
            int batchSize = 10;

            while (hasMoreRecords)
            {
                // Fetch up to 1000 records with flag "Processed" = false
                var recipients = await _context.CampaignRecipient
                    .Where(r => r.Sent == false)
                    .Where(r => r.Campaign.Id == cmd.CampaignIdToRun)
                    .Take(batchSize)
                    .ToListAsync();



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

                        UpdateCampaignRecipients(recipients);
                         
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
            string path = "https://localhost:44385/CampaignProcessor/process";

            var cmd = new ProcessMessageCommand();
            var dtoList = new List<CampaignRecipientDto>();
            
            foreach (var recipient in recipients)
            {
                var r = new CampaignRecipientDto();
                r.Id = recipient.Id;
                r.Email = recipient.Email;
                r.Phone = recipient.Phone;
                r.CampaignIdentifier = recipient.CampaignIdentifier;
                r.Message = recipient.Message;
                r.Sent = recipient.Sent;
                r.SentConfirmed = recipient.SentConfirmed;
                r.ProcessedBy = recipient.ProcessedBy;
                dtoList.Add(r);

            }
             cmd.Recipients = dtoList;

            var serializedData = JsonSerializer.Serialize(cmd);
            var content = new StringContent(serializedData, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(path, content);


            // Send POST request to the broker API
            return response;

        }
 

        [ApiExplorerSettings(IgnoreApi = true)]
        public void UpdateCampaignRecipients(List<CampaignRecipient> recipients)
        {
            if (recipients == null || !recipients.Any())
            {
                throw new ArgumentException("CampaignRecipients list cannot be null or empty.", nameof(recipients));
            }

            try
            {
                foreach (var recipient in recipients)
                {
                    var existingRecipient = _context.CampaignRecipient.Find(recipient.Id);
                    if (existingRecipient != null)
                    {
                        _context.Entry(existingRecipient).CurrentValues.SetValues(recipient);
                    }
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Handle or log exception as needed
                Console.WriteLine($"Error updating campaign recipients: {ex.Message}");
                throw;
            }
        }
    }
}
