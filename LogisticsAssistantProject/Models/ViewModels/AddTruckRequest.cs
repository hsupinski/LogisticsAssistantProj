namespace LogisticsAssistantProject.Models.ViewModels
{
    public class AddTruckRequest
    {
        public required string Name { get; set; }
        public int MaxVelocity { get; set; }
        public int BreakDuration { get; set; }
        public int MinutesUntilBreak { get; set; }
    }
}
