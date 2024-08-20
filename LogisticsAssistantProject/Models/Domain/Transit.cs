namespace LogisticsAssistantProject.Models.Domain
{
    public class Transit
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Distance { get; set; }
    }
}
