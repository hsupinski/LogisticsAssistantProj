using LogisticsAssistantProject.Models.Domain;
using LogisticsAssistantProject.Models.ViewModels;
using LogisticsAssistantProject.Repositories;

namespace LogisticsAssistantProject.Services
{
    public class TransitService : ITransitService
    {
        private readonly ITruckRepository _truckRepository;
        private readonly ITransitRepository _transitRepository;
        public TransitService(ITruckRepository truckRepository, ITransitRepository transitRepository)
        {
            _truckRepository = truckRepository;
            _transitRepository = transitRepository;
        }

        public async Task AddTransitAsync(CreateTransitViewModel model)
        {
            if (model.Distance > 0)
            {
                var truck = await _truckRepository.GetByIdAsync(model.Truck.Id);

                var startTime = model.StartTime;
                var endTime = model.StartTime.AddMinutes(model.Distance / (double)truck.MaxVelocity * 60);

                foreach (var transit in model.TransitList)
                {
                    if (startTime < transit.EndTime && startTime > transit.StartTime || endTime < transit.EndTime && endTime > transit.StartTime)
                    {
                        throw new ArgumentException("Failed to add transit. The transit overlaps with another transit. (" + transit.StartTime + " - " + transit.EndTime + ")");
                    }
                }

                // Distance and minutes until break are both integers
                int amountOfBreaks = model.Distance / truck.MinutesUntilBreak;

                var newTransit = new Transit
                {
                    TruckId = truck.Id,
                    StartTime = startTime,
                    Distance = model.Distance,
                    CreatedAt = DateTime.Now,
                    EndTime = endTime + TimeSpan.FromMinutes(truck.BreakDuration * amountOfBreaks),
                    MaxVelocity = truck.MaxVelocity,
                    BreakDuration = truck.BreakDuration,
                    MinutesUntilBreak = truck.MinutesUntilBreak
                };

                await _transitRepository.AddAsync(newTransit);
            }

            else
            {
                throw new ArgumentException("Failed to add transit. Distance must be greater than 0.");
            }
        }

        public async Task<CreateTransitViewModel> GetTruckTransitsAsync(int truckId)
        {
            var truck = await _truckRepository.GetByIdAsync(truckId);
            var transitList = await _transitRepository.GetByTruckIdAsync(truckId);

            var model = new CreateTransitViewModel
            {
                Truck = truck
            };

            foreach (var transit in transitList)
            {
                model.TransitList.Add(transit);
            }

            return model;
        }
    }
}
