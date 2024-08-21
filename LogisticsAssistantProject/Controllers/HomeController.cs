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
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddTruck()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddTruckRequest request)
        {
            try
            {
                await _truckService.AddTruckAsync(request);
                TempData["SuccessMessage"] = "Truck added successfully!";
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("AddTruck");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListTrucks()
        {
            var trucks = await _truckService.GetAllTrucksAsync();

            return View(trucks);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
