using LogisticsAssistantProject.Models.Domain;
using LogisticsAssistantProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsAssistantProject.Controllers
{
    public class TruckController : Controller
    {
        private readonly ITruckService _truckService;
        private readonly ILogger<TruckController> _logger;
        public TruckController(ITruckService truckService, ILogger<TruckController> logger)
        {
            _truckService = truckService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditTruck(int id)
        {
            _logger.LogInformation("EditTruck page visited");
            return View(await _truckService.GetByIdAsync(id));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditTruck(Truck truck)
        {
            _logger.LogInformation("EditTruck request received");

            try
            {
                await _truckService.UpdateTruckAsync(truck);
                TempData["SuccessMessage"] = "Truck updated successfully!";
                _logger.LogInformation("Truck updated successfully");
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                _logger.LogError(ex, "Failed to update truck");
            }

            return View(truck);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DeleteTruck(int id)
        {
            _logger.LogInformation("DeleteTruck page visited");
            await _truckService.DeleteByIdAsync(id);
            TempData["SuccessMessage"] = "Truck deleted successfully!";

            return RedirectToAction("ListTrucks", "Home");
        }
    }
}
