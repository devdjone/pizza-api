using pizza_api.DTO;
using pizza_api.Models;

namespace pizza_api.Commands
{
    public class ProcessMessageCommand
    {
        public List<CampaignRecipientDto> Recipients { get; set; } = default!;
    }
}
