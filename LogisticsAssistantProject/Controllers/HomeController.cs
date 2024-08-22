using LogisticsAssistantProject.Models;
using LogisticsAssistantProject.Models.ViewModels;
using LogisticsAssistantProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LogisticsAssistantProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITruckService _truckService;

        public HomeController(ILogger<HomeController> logger, ITruckService truckService)
        {
            _logger = logger;
            _truckService = truckService;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Index page visited");
            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("Privacy page visited");
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddTruck()
        {
            _logger.LogInformation("AddTruck page visited");
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddTruckRequest request)
        {
            _logger.LogInformation("AddTruck request received");

            try
            {
                await _truckService.AddTruckAsync(request);
                TempData["SuccessMessage"] = "Truck added successfully!";
                _logger.LogInformation("Truck added successfully");
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                _logger.LogError(ex, "Failed to add truck");
            }

            return RedirectToAction("AddTruck");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListTrucks()
        {
            _logger.LogInformation("ListTrucks page visited");
            var trucks = await _truckService.GetAllTrucksAsync();

            return View(trucks.ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("Error page visited");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
