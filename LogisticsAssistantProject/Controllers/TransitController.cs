using LogisticsAssistantProject.Models.ViewModels;
using LogisticsAssistantProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsAssistantProject.Controllers
{
    public class TransitController : Controller
    {
        private readonly ITransitService _transitService;
        private readonly ILogger<TransitController> _logger;
        public TransitController(ITransitService transitService, ILogger<TransitController> logger)
        {
            _transitService = transitService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateTransit(int truckId)
        {
            _logger.LogInformation("CreateTransit page visited");
            var transits = await _transitService.GetTruckTransitsAsync(truckId);

            return View(transits);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateTransit(CreateTransitViewModel model)
        {
            _logger.LogInformation("CreateTransit request received");

            if (ModelState.IsValid)
            {
                try
                {
                    await _transitService.AddTransitAsync(model);
                    TempData["SuccessMessage"] = "Transit added successfully!";
                    _logger.LogInformation("Transit added successfully");
                }
                catch (ArgumentException ex)
                {
                    _logger.LogError(ex, "Failed to add transit");
                    TempData["ErrorMessage"] = ex.Message;
                }

                return RedirectToAction("CreateTransit", new { truckId = model.Truck.Id });
            }

            _logger.LogError("Failed to add transit. One or more values is not valid.");
            TempData["ErrorMessage"] = "Failed to add transit. One or more values is not valid.";
            return RedirectToAction("CreateTransit", new { truckId = model.Truck.Id });
        }
    }
}
