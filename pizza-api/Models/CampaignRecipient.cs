﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace pizza_api.Models
{
    public class CampaignRecipient
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; } = default!;

        public string Phone { get; set; } = default!;

        public string CampaignIdentifier { get; set; } = default!; // uniqu id 

        public string Message { get; set; } = default!;

        public bool Sent { get; set; } = default!;

        public bool SentConfirmed { get; set; }= default!;

        public string ProcessedBy { get; set; } = default!;


        [JsonIgnore] // Exclude from serialization
        public virtual Campaign Campaign { get; set; } = default!;

    }
}
