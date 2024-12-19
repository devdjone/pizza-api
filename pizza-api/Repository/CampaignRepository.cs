using Microsoft.EntityFrameworkCore;
using pizza_api.Data;
using pizza_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace pizza_api.Repository
{
    public class CampaignRepository : ICampaignRepository
    {
        private readonly pizza_apiContext _context;

        public CampaignRepository(pizza_apiContext context)
        {
            _context = context;
        }

        public void AddCampaign1(Campaign campaign)
        {
            if (campaign == null)
            {
                throw new ArgumentNullException(nameof(campaign));
            }

            try
            {
                _context.Set<Campaign>().Add(campaign);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Handle or log exception as needed
                Console.WriteLine($"Error adding campaign: {ex.Message}");
                throw;
            }
        }

        public void AddCampaign(Campaign campaign)
        {
            if (campaign == null)
            {
                throw new ArgumentNullException(nameof(campaign));
            }

            try
            {
                // Check if the campaign is already being tracked
                var existingCampaign = _context.Set<Campaign>().Local.FirstOrDefault(c => c.Id == campaign.Id);
                if (existingCampaign != null)
                {
                    // If already tracked, no need to add, but just update
                    _context.Entry(existingCampaign).State = EntityState.Modified;
                }
                else
                {
                    // Otherwise, add the new campaign
                    _context.Set<Campaign>().Add(campaign);
                }

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Handle or log exception as needed
                Console.WriteLine($"Error adding campaign: {ex.Message}");
                throw;
            }
        }




        public void AddCampaigns(IEnumerable<Campaign> campaigns)
        {
            if (campaigns == null || !campaigns.Any())
            {
                throw new ArgumentException("Campaigns list cannot be null or empty.", nameof(campaigns));
            }

            try
            {
                _context.Set<Campaign>().AddRange(campaigns);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Handle or log exception as needed
                Console.WriteLine($"Error adding campaigns: {ex.Message}");
                throw;
            }
        }

        public Campaign GetCampaignById(int id)
        {
            return _context.Set<Campaign>().Include(c => c.CampaignRecipient).FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Campaign> GetAllCampaigns()
        {
            return _context.Set<Campaign>().Include(c => c.CampaignRecipient).ToList();
        }

        public void RemoveCampaign(int id)
        {
            var campaign = _context.Set<Campaign>().Find(id);
            if (campaign != null)
            {
                _context.Set<Campaign>().Remove(campaign);
                _context.SaveChanges();
            }
        }

        public void AddCampaignRecipients(List<CampaignRecipient> recipients)
        {
            if (recipients == null || !recipients.Any())
            {
                throw new ArgumentException("CampaignRecipients list cannot be null or empty.", nameof(recipients));
            }

            try
            {
                _context.CampaignRecipient.AddRange(recipients);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Handle or log exception as needed
                Console.WriteLine($"Error adding campaign recipients: {ex.Message}");
                throw;
            }
        }
    }
}
