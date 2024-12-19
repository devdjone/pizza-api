using pizza_api.Models;
using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using pizza_api.Data;

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
                .RuleFor(r => r.Phone, f => f.Phone.PhoneNumber("###-###-####"))
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




        public List<CampaignRecipient> GenerateFakeCampaignRecipients(int campaignId, int numberOfRows)
        {
            if (numberOfRows <= 0)
            {
                throw new ArgumentException("Number of rows must be greater than 0.", nameof(numberOfRows));
            }

            var recipientFaker = new Faker<CampaignRecipient>()
                .RuleFor(r => r.Email, f => f.Internet.Email())  // Generate a fake email
                .RuleFor(r => r.Phone, f => f.Phone.PhoneNumber("###-###-####"))  // Generate a fake phone number
                .RuleFor(r => r.CampaignIdentifier, f => Guid.NewGuid().ToString())  // Generate a unique campaign identifier (GUID)
                .RuleFor(r => r.Message, f => f.Lorem.Sentence())  // Generate a fake message (sentence)
                .RuleFor(r => r.Sent, f => false)  // Random boolean for Sent status
                .RuleFor(r => r.SentConfirmed, false)  // Random boolean for SentConfirmed status
                .RuleFor(r => r.ProcessedBy, f => "loader")  // Random name for ProcessedBy
                .RuleFor(r => r.Campaign, f => new Campaign { Id = campaignId });  // Associate the CampaignId

            // Generate the requested number of fake recipients
            var fakeRecipients = recipientFaker.Generate(numberOfRows);

            return fakeRecipients;


        }


        //public List<CampaignRecipient> GenerateFakeCampaignRecipients2(int campaignId, int numberOfRows)
        //{
        //    if (numberOfRows <= 0)
        //    {
        //        throw new ArgumentException("Number of rows must be greater than 0.", nameof(numberOfRows));
        //    }

        //    // Fetch the existing campaign from the database
            

        //    if (existingCampaign == null)
        //    {
        //        throw new InvalidOperationException($"Campaign with ID {campaignId} not found.");
        //    }

        //    var recipientFaker = new Faker<CampaignRecipient>()
        //        .RuleFor(r => r.Email, f => f.Internet.Email())  // Generate a fake email
        //        .RuleFor(r => r.Phone, f => f.Phone.PhoneNumber("###-###-####"))  // Generate a fake phone number
        //        .RuleFor(r => r.CampaignIdentifier, f => Guid.NewGuid().ToString())  // Generate a unique campaign identifier (GUID)
        //        .RuleFor(r => r.Message, f => f.Lorem.Sentence())  // Generate a fake message (sentence)
        //        .RuleFor(r => r.Sent, f => f.Random.Bool())  // Random boolean for Sent status
        //        .RuleFor(r => r.SentConfirmed, f => f.Random.Bool())  // Random boolean for SentConfirmed status
        //        .RuleFor(r => r.ProcessedBy, f => f.Person.FullName)  // Random name for ProcessedBy
        //        .RuleFor(r => r.Campaign, f => );  // Use the existing campaign

        //    // Generate the requested number of fake recipients
        //    var fakeRecipients = recipientFaker.Generate(numberOfRows);

        //    return fakeRecipients;


        //}

    }
}