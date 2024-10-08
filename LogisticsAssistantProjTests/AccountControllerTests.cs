﻿using LogisticsAssistantProject.Controllers;
using LogisticsAssistantProject.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using LogisticsAssistantProject.Models.ViewModels;
using Microsoft.Extensions.Logging;
using FluentAssertions;

namespace LogisticsAssistantProjTests
{
    public class AccountControllerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<ILogger<AccountController>> _loggerMock;
        private readonly AccountController _accountController;
        private readonly RegisterViewModel exampleRegisterViewModel;
        private readonly LoginViewModel exampleLoginViewModel;

        public AccountControllerTests()
        {
            var accountService = new TestAccountService();
            _loggerMock = new Mock<ILogger<AccountController>>();
            _accountController = new AccountController(accountService, _loggerMock.Object);

            exampleRegisterViewModel = new RegisterViewModel
            {
                Username = "test",
                Email = "test@example.com",
                Password = "Test123!"
            };

            exampleLoginViewModel = new LoginViewModel
            {
                Username = "test",
                Password = "Test123!"
            };
        }

        [Fact]
        public void Register_ReturnsViewResult()
        {
            // Act
            var result = _accountController.Register();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task Register_ReturnsRedirectToActionResult_OnSuccessfulRegister()
        {
            // Arrange
            var model = exampleRegisterViewModel;

            // Act
            var result = await _accountController.Register(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>()
                .Which.ActionName.Should().Be("Index");

            result.As<RedirectToActionResult>().ControllerName.Should().Be("Home");
        }

        [Fact]
        public async Task Register_ReturnsViewResult_WhenRegistrationFails()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Username = "invalid",
                Email = "invalid@example.com",
                Password = "Invalid123!"
            };

            // Act
            var result = await _accountController.Register(model);

            // Assert
            result.Should().BeOfType<ViewResult>()
                .Which.Model.Should().Be(model);
        }

        [Fact]
        public async Task Login_ReturnsViewResult()
        {
            // Act
            var result = _accountController.Login();

            // Assert
            result.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task Login_ReturnsRedirectToActionResult_OnSuccessfulLogin()
        {
            // Arrange
            var model = exampleLoginViewModel;

            // Act
            var result = await _accountController.Login(model);

            // Assert
            result.Should().BeOfType<RedirectToActionResult>()
                .Which.ActionName.Should().Be("Index");

            result.As<RedirectToActionResult>().ControllerName.Should().Be("Home");
        }

        [Fact]
        public async Task Login_ReturnsRedirectToActionResult_WhenLoginFails()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Username = "invalid",
                Password = "Invalid123!"
            };

            // Act
            var result = await _accountController.Login(model);

            // Assert
            result.Should().BeOfType<ViewResult>()
                .Which.Model.Should().Be(model);

            _accountController.ModelState.Should().ContainKey("")
                .WhoseValue.Errors[0].ErrorMessage.Should().Be("Invalid Login Attempt");
        }

        [Fact]
        public async Task Logout_ReturnsRedirectToActionResult()
        {
            // Act
            var result = await _accountController.Logout();

            // Assert
            result.Should().BeOfType<RedirectToActionResult>()
                .Which.ActionName.Should().Be("Index");

            result.As<RedirectToActionResult>().ControllerName.Should().Be("Home");
        }
    }
}
