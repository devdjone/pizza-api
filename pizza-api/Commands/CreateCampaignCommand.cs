namespace pizza_api.Commands
{
    public class CreateCampaignCommand
    {
        public string Name { get; set; } = default!;

        public DateTime StartTime { get; set; } = default!;
        public int Rows { get; set; } = default!;
    }
}
