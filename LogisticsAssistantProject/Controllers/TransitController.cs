using LogisticsAssistantProject.Models.Domain;
using LogisticsAssistantProject.Models.ViewModels;
using LogisticsAssistantProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsAssistantProject.Controllers
{
    public class TransitController : Controller
    {
        private readonly ITransitRepository _transitRepository;
        private readonly ITruckRepository _truckRepository;
        public TransitController(ITransitRepository transitRepository, ITruckRepository truckRepository)
        {
            _transitRepository = transitRepository;
            _truckRepository = truckRepository;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateTransit(int truckId)
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

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTransit(CreateTransitViewModel model)
        {
            if (ModelState.IsValid && model.Distance > 0)
            {
                var truck = await _truckRepository.GetByIdAsync(model.Truck.Id);

                var startTime = model.StartTime;
                var endTime = model.StartTime.AddMinutes(model.Distance / (double)truck.MaxVelocity * 60);

                foreach(var transit in model.TransitList)
                {
                    if (startTime < transit.EndTime && startTime > transit.StartTime || endTime < transit.EndTime && endTime > transit.StartTime)
                    {
                        TempData["ErrorMessage"] = "Failed to add transit. The transit overlaps with another transit. (" + transit.StartTime + " - " + transit.EndTime + ")";
                        return RedirectToAction("CreateTransit", new { truckId = model.Truck.Id });
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
                TempData["SuccessMessage"] = "Transit added successfully!";

                return RedirectToAction("CreateTransit", new { truckId = model.Truck.Id });
            }

            TempData["ErrorMessage"] = "Failed to add transit. One or more values is not valid.";
            return RedirectToAction("CreateTransit", new { truckId = model.Truck.Id });
        }
    }
}
