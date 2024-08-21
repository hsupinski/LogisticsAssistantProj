using LogisticsAssistantProject.Models.Domain;
using LogisticsAssistantProject.Models.ViewModels;
using System.Collections.Generic;

namespace LogisticsAssistantProject.Services
{
    public interface ITruckService
    {
        Task AddTruckAsync(AddTruckRequest request);
        Task<IEnumerable<Truck>> GetAllTrucksAsync();
        Task<Truck> GetByIdAsync(int id);
        Task UpdateTruckAsync(Truck truck);
        Task DeleteByIdAsync(int id);
    }
}
