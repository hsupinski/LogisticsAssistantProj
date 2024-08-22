using FluentAssertions;
using LogisticsAssistantProject.Controllers;
using LogisticsAssistantProject.Models.Domain;
using LogisticsAssistantProject.Models.ViewModels;
using LogisticsAssistantProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;

namespace LogisticsAssistantProjTests
{
    public class HomeControllerTests
    {
        private readonly Mock<ITruckService> _truckServiceMock;
        private readonly HomeController _homeController;
        private readonly Mock<ILogger<HomeController>> _loggerMock;

        public HomeControllerTests()
        {
            _loggerMock = new Mock<ILogger<HomeController>>();
            _truckServiceMock = new Mock<ITruckService>();
            _homeController = new HomeController(_loggerMock.Object, _truckServiceMock.Object);

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _homeController.TempData = tempData;
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _homeController.Index();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void AddTruck_ReturnsViewResult()
        {
            // Act
            var result = _homeController.AddTruck();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task Add_ReturnsRedirectToActionResult_WhenTruckIsValid()
        {
            // Arrange
            var request = new AddTruckRequest
            {
                MaxVelocity = 100,
                BreakDuration = 10,
                MinutesUntilBreak = 60
            };

            // Act
            var result = await _homeController.Add(request);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>()
                .Which.ActionName.Should().Be("AddTruck");

            _homeController.TempData.Should().ContainKey("SuccessMessage");
        }

        [Fact]
        public async Task Add_ReturnsRedirectToActionResult_WhenTruckIsInvalid()
        {
            // Arrange
            var request = new AddTruckRequest
            {
                MaxVelocity = 0,
                BreakDuration = 0,
                MinutesUntilBreak = 0
            };

            _truckServiceMock.Setup(x => x.AddTruckAsync(request))
                .ThrowsAsync(new ArgumentException("Invalid truck data"));

            // Act
            var result = await _homeController.Add(request);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>()
                .Which.ActionName.Should().Be("AddTruck");

            _homeController.TempData.Should().ContainKey("ErrorMessage");
        }

        [Fact]
        public async Task ListTrucks_ReturnsViewResultWithTruckList()
        {
            // Arrange
            var trucks = new List<Truck>
            {
                new Truck { Id = 1, MaxVelocity = 100, BreakDuration = 10, MinutesUntilBreak = 60 },
                new Truck { Id = 2, MaxVelocity = 120, BreakDuration = 15, MinutesUntilBreak = 45 }
            };

            _truckServiceMock.Setup(x => x.GetAllTrucksAsync())
                .ReturnsAsync(trucks);

            // Act
            var result = await _homeController.ListTrucks();

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeOfType<List<Truck>>().Subject;
            model.Count.Should().Be(trucks.Count);
        }
    }
}
