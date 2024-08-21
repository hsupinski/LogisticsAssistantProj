using LogisticsAssistantProject.Data;
using LogisticsAssistantProject.Models.Domain;
using LogisticsAssistantProject.Repositories;
using LogisticsAssistantProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsAssistantProject.Controllers
{
    public class TruckController : Controller
    {
        private readonly ITruckService _truckService;
        public TruckController(ITruckService truckService)
        {
            _truckService = truckService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditTruck(int id)
        {
            return View(await _truckService.GetByIdAsync(id));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditTruck(Truck truck)
        {
            try
            {
                await _truckService.UpdateTruckAsync(truck);
                TempData["SuccessMessage"] = "Truck updated successfully!";
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return View(truck);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DeleteTruck(int id)
        {
            await _truckService.DeleteByIdAsync(id);
            TempData["SuccessMessage"] = "Truck deleted successfully!";

            return RedirectToAction("ListTrucks", "Home");
        }
    }
}
