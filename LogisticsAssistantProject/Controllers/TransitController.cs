using LogisticsAssistantProject.Models.ViewModels;
using LogisticsAssistantProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsAssistantProject.Controllers
{
    public class TransitController : Controller
    {
        private readonly ITransitService _transitService;
        public TransitController(ITransitService transitService)
        {
            _transitService = transitService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateTransit(int truckId)
        {
            var transits = await _transitService.GetTruckTransitsAsync(truckId);

            return View(transits);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTransit(CreateTransitViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _transitService.AddTransitAsync(model);
                    TempData["SuccessMessage"] = "Transit added successfully!";
                }
                catch (ArgumentException ex)
                {
                    TempData["ErrorMessage"] = ex.Message;
                }

                return RedirectToAction("CreateTransit", new { truckId = model.Truck.Id });
            }

            TempData["ErrorMessage"] = "Failed to add transit. One or more values is not valid.";
            return RedirectToAction("CreateTransit", new { truckId = model.Truck.Id });
        }
    }
}
