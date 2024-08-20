namespace LogisticsAssistantProject.Models.Domain
{
    public class Transit
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int TruckId { get; set; }
        public int MaxVelocity { get; set; }
        public int BreakDuration { get; set; }
        public int MinutesUntilBreak { get; set; }
        public int Distance { get; set; }
    }
}
