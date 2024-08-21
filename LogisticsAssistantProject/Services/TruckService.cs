using LogisticsAssistantProject.Models.Domain;
using LogisticsAssistantProject.Models.ViewModels;
using LogisticsAssistantProject.Repositories;
using System.Diagnostics;

namespace LogisticsAssistantProject.Services
{
    public class TruckService : ITruckService
    {
        private readonly ITruckRepository _truckRepository;
        public TruckService(ITruckRepository truckRepository)
        {
            _truckRepository = truckRepository;
        }

        public async Task AddTruckAsync(AddTruckRequest request)
        {
            var truck = new Truck
            {
                MaxVelocity = request.MaxVelocity,
                BreakDuration = request.BreakDuration,
                MinutesUntilBreak = request.MinutesUntilBreak
            };

            if(truck.MaxVelocity <= 0 || truck.BreakDuration <= 0 || truck.MinutesUntilBreak <= 0)
            {
                throw new ArgumentException("Failed to add truck. One or more values is not valid.");
            }

            await _truckRepository.AddAsync(truck);
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _truckRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Truck>> GetAllTrucksAsync()
        {
            return await _truckRepository.GetAllAsync();
        }

        public async Task<Truck> GetByIdAsync(int id)
        {
            return await _truckRepository.GetByIdAsync(id);
        }

        public async Task UpdateTruckAsync(Truck truck)
        {
            if(truck.MaxVelocity <= 0 || truck.BreakDuration <= 0 || truck.MinutesUntilBreak <= 0)
            {
                throw new ArgumentException("Failed to update truck. One or more values is not valid.");
            }

            await _truckRepository.UpdateAsync(truck);

        }
    }
}
