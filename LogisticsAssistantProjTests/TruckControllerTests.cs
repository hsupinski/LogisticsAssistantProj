﻿using FluentAssertions;
using LogisticsAssistantProject.Controllers;
using LogisticsAssistantProject.Models.Domain;
using LogisticsAssistantProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;

namespace LogisticsAssistantProjTests
{
    public class TruckControllerTests
    {
        private readonly Mock<ITruckService> _truckServiceMock;
        private readonly Mock<ILogger<TruckController>> _loggerMock;
        private readonly TruckController _truckController;
        private readonly Truck exampleTruck;

        public TruckControllerTests()
        {
            _truckServiceMock = new Mock<ITruckService>();
            _loggerMock = new Mock<ILogger<TruckController>>();
            _truckController = new TruckController(_truckServiceMock.Object, _loggerMock.Object);

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
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeOfType<Truck>().Subject;
            model.Id.Should().Be(truck.Id);
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
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeOfType<Truck>().Subject;
            model.Id.Should().Be(truck.Id);
            _truckController.TempData.Should().ContainKey("SuccessMessage");
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
            var viewResult = result.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeOfType<Truck>().Subject;
            model.Id.Should().Be(truck.Id);
            _truckController.TempData.Should().ContainKey("ErrorMessage");
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
            result.Should().BeOfType<RedirectToActionResult>()
                .Which.ActionName.Should().Be("ListTrucks");

            _truckController.TempData.Should().ContainKey("SuccessMessage");
        }
    }
}
