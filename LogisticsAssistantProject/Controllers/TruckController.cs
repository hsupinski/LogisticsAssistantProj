﻿using LogisticsAssistantProject.Data;
using LogisticsAssistantProject.Models.Domain;
using LogisticsAssistantProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsAssistantProject.Controllers
{
    public class TruckController : Controller
    {
        private readonly ITruckRepository _truckRepository;
        public TruckController(ITruckRepository truckRepository)
        {
            _truckRepository = truckRepository;
        }

        [HttpGet]
        public async Task<IActionResult> EditTruck(int id)
        {
            var truck = await _truckRepository.GetByIdAsync(id);
            return View(truck);
        }

        [HttpPost]
        public async Task<IActionResult> EditTruck(Truck truck)
        {

            if (truck.MaxVelocity <= 0 || truck.BreakDuration <= 0 || truck.MinutesUntilBreak <= 0)
            {
                TempData["ErrorMessage"] = "Failed to update truck. One or more values is not valid.";
                return RedirectToAction("ListTrucks");
            }

            await _truckRepository.UpdateAsync(truck);

            TempData["SuccessMessage"] = "Truck updated successfully!";

            return View(truck);
        }

    }
}
