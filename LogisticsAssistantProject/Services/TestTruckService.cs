using LogisticsAssistantProject.Models.Domain;
using LogisticsAssistantProject.Models.ViewModels;

namespace LogisticsAssistantProject.Services
{
    public class TestTruckService : ITruckService
    {
        public Task AddTruckAsync(AddTruckRequest request)
        {
            if (request.MaxVelocity <= 0 || request.BreakDuration <= 0 || request.MinutesUntilBreak <= 0)
            {
                throw new ArgumentException("Invalid truck data");
            }

            return Task.CompletedTask;
        }

        public Task DeleteByIdAsync(int id)
        {
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Truck>> GetAllTrucksAsync()
        {
            var trucks = new List<Truck>
            {
                new Truck { Id = 1, MaxVelocity = 100, BreakDuration = 10, MinutesUntilBreak = 60 },
                new Truck { Id = 2, MaxVelocity = 120, BreakDuration = 15, MinutesUntilBreak = 45 }
            };

            return Task.FromResult(trucks.AsEnumerable());
        }

        public Task<Truck> GetByIdAsync(int id)
        {
            var truck = new Truck { Id = id, MaxVelocity = 100, BreakDuration = 10, MinutesUntilBreak = 60 };
            return Task.FromResult(truck);
        }

        public Task UpdateTruckAsync(Truck truck)
        {
            if(truck.MaxVelocity <= 0 || truck.BreakDuration <= 0 || truck.MinutesUntilBreak <= 0)
            {
                throw new ArgumentException("Invalid truck data");
            }

            return Task.CompletedTask;
        }
    }
}
