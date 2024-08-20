namespace LogisticsAssistantProject.Models.Domain
{
    public class TruckInTransit
    {
        public int Id { get; set; }
        public int TruckId { get; set; }
        public int TransitId { get; set; }
    }
}
