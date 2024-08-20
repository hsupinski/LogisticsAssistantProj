using LogisticsAssistantProject.Models.Domain;

namespace LogisticsAssistantProject.Repositories
{
    public interface ITruckRepository
    {
        Task<IEnumerable<Truck>> GetAllAsync();
        Task<Truck> GetByIdAsync(int id);
        Task<Truck> AddAsync(Truck truck);
        Task<Truck> UpdateAsync(Truck truck);
    }
}
