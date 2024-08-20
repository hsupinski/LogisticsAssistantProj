using LogisticsAssistantProject.Models.Domain;

namespace LogisticsAssistantProject.Repositories
{
    public interface ITruckRepository
    {
        Task<IEnumerable<Truck>> GetAllAsync();
        Task<Truck> AddAsync(Truck truck);
    }
}
