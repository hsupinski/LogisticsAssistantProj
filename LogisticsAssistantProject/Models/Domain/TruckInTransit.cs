namespace LogisticsAssistantProject.Models.Domain
{
    public class TruckInTransit
    {
        public int Id { get; set; }
        public int TransitId { get; set; }
        public int TruckId { get; set; }
        // Keep the properties below in case they change later on
        public int MaxVelocity { get; set; }
        public int BreakDuration { get; set; }
        public int MinutesUntilBreak { get; set; }
    }
}
