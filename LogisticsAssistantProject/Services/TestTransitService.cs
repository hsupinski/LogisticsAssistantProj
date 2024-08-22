using LogisticsAssistantProject.Models.Domain;
using LogisticsAssistantProject.Models.ViewModels;

namespace LogisticsAssistantProject.Services
{
    public class TestTransitService : ITransitService
    {
        public Task AddTransitAsync(CreateTransitViewModel model)
        {
            if(model.Distance <= 0)
            {
                throw new ArgumentException("Failed to add transit. Distance must be greater than 0.");
            }

            else
            {
                var truck = model.Truck;

                var startTime = model.StartTime;
                var endTime = model.StartTime.AddMinutes(model.Distance / (double)truck.MaxVelocity * 60);

                foreach (var transit in model.TransitList)
                {
                    if (startTime < transit.EndTime && startTime > transit.StartTime || endTime < transit.EndTime && endTime > transit.StartTime)
                    {
                        throw new ArgumentException("Failed to add transit. The transit overlaps with another transit. (" + transit.StartTime + " - " + transit.EndTime + ")");
                    }
                }
            }

            return Task.FromResult(model);
        }

        public Task<CreateTransitViewModel> GetTruckTransitsAsync(int truckId)
        {
            var transitList = new List<Transit>
            {
                new Transit
                {
                    Id = 1,
                    TruckId = truckId,
                    StartTime = DateTime.Now,
                    Distance = 100,
                    CreatedAt = DateTime.Now,
                    EndTime = DateTime.Now.AddHours(1),
                    MaxVelocity = 100,
                    BreakDuration = 10,
                    MinutesUntilBreak = 60
                },
                new Transit
                {
                    Id = 2,
                    TruckId = truckId,
                    StartTime = DateTime.Now.AddHours(2),
                    Distance = 100,
                    CreatedAt = DateTime.Now,
                    EndTime = DateTime.Now.AddHours(3),
                    MaxVelocity = 100,
                    BreakDuration = 10,
                    MinutesUntilBreak = 60
                }
            };

            var model = new CreateTransitViewModel
            {
                Truck = new Truck
                {
                    Id = truckId,
                    MaxVelocity = 100,
                    BreakDuration = 10,
                    MinutesUntilBreak = 60
                },
                TransitList = transitList,
                StartTime = DateTime.Now,
                Distance = 100
            };

            return Task.FromResult(model);
        }
    }
}
