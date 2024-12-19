using System;
using System.ComponentModel.DataAnnotations;

namespace pizza_api.Models
{
    public class Campaign
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public DateTime StartDate { get; set; } = default!;

        public string Status { get; set; } = default!;

        public virtual ICollection<CampaignRecipient> CampaignRecipient { get; set; } = default!;


    }
}
