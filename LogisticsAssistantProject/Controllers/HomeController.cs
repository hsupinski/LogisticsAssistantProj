using LogisticsAssistantProject.Models;
using LogisticsAssistantProject.Models.Domain;
using LogisticsAssistantProject.Models.ViewModels;
using LogisticsAssistantProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LogisticsAssistantProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITruckRepository _truckRepository;

        public HomeController(ILogger<HomeController> logger, ITruckRepository truckRepository)
        {
            _logger = logger;
            _truckRepository = truckRepository;
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
            var truck = new Truck
            {
                MaxVelocity = request.MaxVelocity,
                BreakDuration = request.BreakDuration,
                MinutesUntilBreak = request.MinutesUntilBreak
            };

            if(truck.MaxVelocity <= 0 || truck.BreakDuration <= 0 || truck.MinutesUntilBreak <= 0)
            {
                TempData["ErrorMessage"] = "Failed to add truck. One or more values is not valid.";
                return RedirectToAction("AddTruck");
            }


            await _truckRepository.AddAsync(truck);

            TempData["SuccessMessage"] = "Truck added successfully!";

            return RedirectToAction("AddTruck");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListTrucks()
        {
            var trucks = await _truckRepository.GetAllAsync();

            var model = new List<Truck>();

            foreach(var truck in trucks)
            {
                model.Add(truck);
            }

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
