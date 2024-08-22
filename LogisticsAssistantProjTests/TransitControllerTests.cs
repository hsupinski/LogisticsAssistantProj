using Castle.Core.Logging;
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
    public class TransitControllerTests
    {
        private readonly Mock<ITransitService> _transitServiceMock;
        private readonly Mock<ILogger<TransitController>> _loggerMock;
        private readonly TransitController _transitController;
        private readonly List<Transit> validTransitList;
        private readonly List<Transit> overlapingTransitList;
        private readonly Truck exampleTruck;

        public TransitControllerTests()
        {
            _transitServiceMock = new Mock<ITransitService>();
            _loggerMock = new Mock<ILogger<TransitController>>();
            _transitController = new TransitController(_transitServiceMock.Object, _loggerMock.Object);

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _transitController.TempData = tempData;

            validTransitList = new List<Transit>
            {
                new Transit
                {
                    TruckId = 1,
                    StartTime = DateTime.Now,
                    Distance = 100,
                    CreatedAt = DateTime.Now,
                    EndTime = DateTime.Now.AddHours(1),
                    MaxVelocity = 100,
                    BreakDuration = 10,
                    MinutesUntilBreak = 60
                },
                new Transit
                {
                    TruckId = 1,
                    StartTime = DateTime.Now.AddHours(2),
                    Distance = 100,
                    CreatedAt = DateTime.Now,
                    EndTime = DateTime.Now.AddHours(3),
                    MaxVelocity = 100,
                    BreakDuration = 10,
                    MinutesUntilBreak = 60
                }
            };

            overlapingTransitList = new List<Transit>
            {
                new Transit
                {
                    TruckId = 1,
                    StartTime = DateTime.Now,
                    Distance = 100,
                    CreatedAt = DateTime.Now,
                    EndTime = DateTime.Now.AddHours(1),
                    MaxVelocity = 100,
                    BreakDuration = 10,
                    MinutesUntilBreak = 60
                },
                new Transit
                {
                    TruckId = 1,
                    StartTime = DateTime.Now.AddMinutes(30),
                    Distance = 100,
                    CreatedAt = DateTime.Now.AddMinutes(30),
                    EndTime = DateTime.Now.AddHours(1).AddMinutes(30),
                    MaxVelocity = 100,
                    BreakDuration = 10,
                    MinutesUntilBreak = 60
                }
            };

            exampleTruck = new Truck
            {
                Id = 1,
                MaxVelocity = 100,
                BreakDuration = 10,
                MinutesUntilBreak = 60
            };
        }

        [Fact]
        public async Task CreateTransit_ReturnsViewResultWithModel()
        {
            // Arrange
            var truckId = 1;

            var truck = exampleTruck;

            var transitList = validTransitList;

            var model = new CreateTransitViewModel
            {
                Truck = truck,
                TransitList = transitList
            };

            _transitServiceMock.Setup(t => t.GetTruckTransitsAsync(truckId)).ReturnsAsync(model);

            // Act
            var result = await _transitController.CreateTransit(truckId);

            // Assert
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var receivedModel = viewResult.Model.Should().BeOfType<CreateTransitViewModel>().Subject;
            receivedModel.Should().BeEquivalentTo(model);
            receivedModel.TransitList.Count.Should().Be(model.TransitList.Count);
        }

        [Fact]
        public async Task CreateTransit_ReturnsViewResult_WhenTransitOverlaps()
        {
            // Arrange
            var truckId = 1;
            var truck = exampleTruck;
            var transitList = overlapingTransitList;

            var model = new CreateTransitViewModel
            {
                Truck = truck,
                TransitList = transitList
            };

            _transitServiceMock.Setup(t => t.GetTruckTransitsAsync(truckId)).ReturnsAsync(model);
            _transitServiceMock.Setup(t => t.AddTransitAsync(It.IsAny<CreateTransitViewModel>()))
                               .ThrowsAsync(new ArgumentException("Transit overlaps with an existing one"));

            // Act
            var result = await _transitController.CreateTransit(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>()
                .Which.ActionName.Should().Be("CreateTransit");

            _transitController.TempData.Should().ContainKey("ErrorMessage");
        }

        [Fact]
        public async Task CreateTransit_ReturnsRedirectToActionResult_WhenModelIsInvalid()
        {
            // Arrange
            var truck = exampleTruck;

            var model = new CreateTransitViewModel
            {
                Truck = truck,
                TransitList = validTransitList,
                Distance = 0, // Invalid distance
                StartTime = DateTime.Now
            };

            _transitController.ModelState.AddModelError("Distance", "Invalid distance");

            _transitServiceMock.Setup(t => t.GetTruckTransitsAsync(truck.Id)).ReturnsAsync(model);

            // Act
            var result = await _transitController.CreateTransit(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>()
                .Which.ActionName.Should().Be("CreateTransit");

            _transitController.TempData.Should().ContainKey("ErrorMessage");
        }
    }
}
