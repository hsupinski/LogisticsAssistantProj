using LogisticsAssistantProject.Models.Domain;

namespace LogisticsAssistantProject.Models.ViewModels
{
    public class CreateTransitViewModel
    {
        public Truck Truck { get; set; }
        public List<Transit> TransitList { get; set; } = new List<Transit>();

        public DateTime StartTime { get; set; }
        public int Distance { get; set; }
    }
}