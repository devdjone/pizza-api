using pizza_api.Models;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pizza_api.Services
{
    public class CampaignDataService
    {
        public List<Campaign> GenerateCampaignData(string campaignName, int numberOfRows)
        {
            if (numberOfRows <= 0)
            {
                throw new ArgumentException("Number of rows must be greater than 0.", nameof(numberOfRows));
            }

            // Create a unique identifier for the campaign
            var campaignId = 1; // This could be dynamically generated based on context
            var campaign = new Campaign
            {
                Name = campaignName,
                StartDate = DateTime.Now.AddDays(2),
                Status = "Active",
                CampaignRecipient = new List<CampaignRecipient>()
            };

            // Generate recipient data using Bogus
            var recipientFaker = new Faker<CampaignRecipient>()
                //.RuleFor(r => r.Id, f => f.IndexFaker + 1)
                .RuleFor(r => r.Email, f => f.Internet.Email())
                .RuleFor(r => r.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(r => r.CampaignIdentifier, f => Guid.NewGuid().ToString())
                .RuleFor(r => r.Message, f => f.Lorem.Sentence())
                .RuleFor(r => r.Sent, f => false)
                .RuleFor(r => r.SentConfirmed, false)
                .RuleFor(r => r.ProcessedBy, "loader")
                
                .RuleFor(r => r.Campaign, campaign);

            // Add the generated recipients to the campaign
            campaign.CampaignRecipient = recipientFaker.Generate(numberOfRows);

            // Return the generated campaign with its recipients
            return new List<Campaign> { campaign };
        }
    }
}
