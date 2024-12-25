namespace pizza_api.Commands
{
    public class ActivateCampaignCommand
    {
        public int CampaignIdToRun { get; set; }
        public string ProcessorUrl { get; set; } = default!;
    }
}
