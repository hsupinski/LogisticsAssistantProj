using LogisticsAssistantProject.Models.ViewModels;

namespace LogisticsAssistantProject.Services
{
    public interface ITransitService
    {
        Task<CreateTransitViewModel> GetTruckTransitsAsync(int truckId);
        Task AddTransitAsync(CreateTransitViewModel model);

    }
}
