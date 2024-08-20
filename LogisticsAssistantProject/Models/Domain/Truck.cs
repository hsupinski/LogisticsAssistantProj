namespace LogisticsAssistantProject.Models.Domain
{
    public class Truck
    {
        public int Id { get; set; }
        public int MaxVelocity { get; set; }
        public int BreakDuration { get; set; } 
        public int MinutesUntilBreak { get; set; }
    }
}
