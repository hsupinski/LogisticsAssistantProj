using LogisticsAssistantProject.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsAssistantProject.Repositories
{
    public interface ITruckRepository
    {
        Task<IEnumerable<Truck>> GetAllAsync();
        Task<Truck> GetByIdAsync(int id);
        Task<Truck> AddAsync(Truck truck);
        Task<Truck> UpdateAsync(Truck truck);
        Task<IActionResult> DeleteAsync(int id);
    }
}
