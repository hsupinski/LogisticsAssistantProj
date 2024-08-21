using LogisticsAssistantProject.Controllers;
using LogisticsAssistantProject.Models.Domain;
using LogisticsAssistantProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;

namespace LogisticsAssistantProjTests
{
    public class TruckControllerTests
    {
        private readonly Mock<ITruckService> _truckServiceMock;
        private readonly TruckController _truckController;
        private readonly Truck exampleTruck;

        public TruckControllerTests()
        {
            _truckServiceMock = new Mock<ITruckService>();
            _truckController = new TruckController(_truckServiceMock.Object);

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _truckController.TempData = tempData;

            exampleTruck = new Truck
            {
                Id = 1,
                MaxVelocity = 100,
                BreakDuration = 10,
                MinutesUntilBreak = 60
            };
        }

        [Fact]
        public async Task EditTruck_ReturnsViewResultWithTruck()
        {
            // Arrange
            var truck = exampleTruck;

            _truckServiceMock.Setup(s => s.GetByIdAsync(truck.Id)).ReturnsAsync(truck);

            // Act
            var result = await _truckController.EditTruck(truck.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Truck>(viewResult.Model);
            Assert.Equal(truck.Id, model.Id);
        }

        [Fact]
        public async Task EditTruck_ReturnsViewResult_WhenModelIsValid()
        {
            // Arrange
            var truck = exampleTruck;

            _truckServiceMock.Setup(s => s.UpdateTruckAsync(truck)).Returns(Task.CompletedTask);

            // Act
            var result = await _truckController.EditTruck(truck);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Truck>(viewResult.Model);
            Assert.Equal(truck.Id, model.Id);
            Assert.True(_truckController.TempData.ContainsKey("SuccessMessage"));
        }

        [Fact]
        public async Task EditTruck_ReturnsViewResult_WhenModelIsInvalid()
        {
            // Arrange
            var truck = new Truck
            {
                Id = 1,
                MaxVelocity = 0, // Invalid
                BreakDuration = 0, // Invalid
                MinutesUntilBreak = 0 // Invalid
            };

            _truckServiceMock.Setup(s => s.UpdateTruckAsync(truck))
                             .ThrowsAsync(new ArgumentException("Invalid truck data"));

            // Act
            var result = await _truckController.EditTruck(truck);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Truck>(viewResult.Model);
            Assert.Equal(truck.Id, model.Id);
            Assert.True(_truckController.TempData.ContainsKey("ErrorMessage"));
        }

        [Fact]
        public async Task DeleteTruck_ReturnsRedirectToActionResult() 
        {
            // Arrange
            var truck = exampleTruck;

            _truckServiceMock.Setup(s => s.DeleteByIdAsync(truck.Id)).Returns(Task.CompletedTask);

            // Act
            var result = await _truckController.DeleteTruck(truck.Id);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ListTrucks", redirectToActionResult.ActionName);
            Assert.True(_truckController.TempData.ContainsKey("SuccessMessage"));
        }
    }
}
