using LogisticsAssistantProject.Controllers;
using LogisticsAssistantProject.Models.Domain;
using LogisticsAssistantProject.Models.ViewModels;
using LogisticsAssistantProject.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;

namespace LogisticsAssistantProjTests
{
    public class AccountControllerTests
    {
        private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
        private readonly Mock<SignInManager<IdentityUser>> _signInManagerMock;
        private readonly AccountController _accountController;

        public AccountControllerTests()
        {
            _userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                null, null, null, null, null, null, null, null);

            _signInManagerMock = new Mock<SignInManager<IdentityUser>>(
                _userManagerMock.Object,
                Mock.Of<IHttpContextAccessor>(),
                Mock.Of<IUserClaimsPrincipalFactory<IdentityUser>>(),
                null, null, null, null);

            _accountController = new AccountController(_userManagerMock.Object, _signInManagerMock.Object);
        }

        [Fact]
        public void Register_ReturnsViewResult()
        {
            // Arrange
            var controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object);

            // Act
            var result = controller.Register();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Register_ReturnsRedirectToActionResult_OnSuccessfulRegister()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Username = "test",
                Email = "test@test.com",
                Password = "Test123!"

            };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _accountController.Register(model);

            // Assert

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
        }

        [Fact]
        public async Task Register_ReturnsViewResult_WhenRegistrationFails()
        {
            // Arrange
            var model = new RegisterViewModel
            {
                Username = "test",
                Email = "test@test.com",
                Password = "Test123!"
            };

            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

            // Act
            var result = await _accountController.Register(model);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var returnedModel = Assert.IsType<RegisterViewModel>(viewResult.Model);
            Assert.Equal(model, returnedModel);
            Assert.True(_accountController.ModelState.ContainsKey(string.Empty));
        }

        [Fact]
        public void Login_ReturnsViewResult()
        {
            // Arrange
            var controller = new AccountController(_userManagerMock.Object, _signInManagerMock.Object);

            // Act
            var result = controller.Login();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsRedirectToActionResult_OnSuccessfulLogin()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Username = "test",
                Password = "Test123!"
            };

            _signInManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            // Act
            var result = await _accountController.Login(model);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
        }

        [Fact]
        public async Task Login_ReturnsRedirectToActionResult_WhenLoginFails()
        {
            // Arrange
            var model = new LoginViewModel
            {
                Username = "test",
                Password = "Test123!"
            };

            _signInManagerMock.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Act
            var result = await _accountController.Login(model);

            // Assert
            var redirectToActionResult = Assert.IsType<ViewResult>(result);
            var returnedModel = Assert.IsType<LoginViewModel>(redirectToActionResult.Model);
            Assert.Equal(model, returnedModel);
            Assert.True(_accountController.ModelState.ContainsKey(string.Empty));
        }

        [Fact]
        public async Task Logout_ReturnsRedirectToActionResult()
        {
            // Arrange
            _signInManagerMock.Setup(x => x.SignOutAsync())
                .Returns(Task.CompletedTask);

            // Act
            var result = await _accountController.Logout();

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
        }
    }

    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _loggerMock;
        private readonly Mock<ITruckRepository> _truckRepositoryMock;
        private readonly HomeController _homeController;

        public HomeControllerTests()
        {
            _loggerMock = new Mock<ILogger<HomeController>>();
            _truckRepositoryMock = new Mock<ITruckRepository>();
            _homeController = new HomeController(_loggerMock.Object, _truckRepositoryMock.Object);

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _homeController.TempData = tempData;
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var controller = new HomeController(_loggerMock.Object, _truckRepositoryMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void AddTruck_ReturnsViewResult()
        {
            // Arrange
            var controller = new HomeController(_loggerMock.Object, _truckRepositoryMock.Object);

            // Act
            var result = controller.AddTruck();

            // Assert
            Assert.IsType<ViewResult>(result);
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
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("AddTruck", redirectToActionResult.ActionName);
            Assert.True(_homeController.TempData.ContainsKey("SuccessMessage"));

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

            // Act
            var result = await _homeController.Add(request);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("AddTruck", redirectToActionResult.ActionName);
            Assert.True(_homeController.TempData.ContainsKey("ErrorMessage"));
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

            _truckRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(trucks);

            // Act
            var result = await _homeController.ListTrucks();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<List<Truck>>(viewResult.Model);
            Assert.Equal(trucks.Count, model.Count);
        }
    }

    public class TransitControllerTests
    {
        private readonly Mock<ITruckRepository> _truckRepositoryMock;
        private readonly Mock<ITransitRepository> _transitRepositoryMock;
        private readonly TransitController _transitController;

        public TransitControllerTests()
        {
            _transitRepositoryMock = new Mock<ITransitRepository>();
            _truckRepositoryMock = new Mock<ITruckRepository>();
            _transitController = new TransitController(_transitRepositoryMock.Object, _truckRepositoryMock.Object);

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _transitController.TempData = tempData;
        }

        [Fact]
        public async Task CreateTransit_ReturnsViewResultWithModel()
        {
            // Arrange
            var truck = new Truck
            {
                Id = 1,
                MaxVelocity = 100,
                BreakDuration = 10,
                MinutesUntilBreak = 60
            };

            var transitList = new List<Transit>
            {
                new Transit { Id = 1, TruckId = 1, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) },
                new Transit { Id = 2, TruckId = 1, StartTime = DateTime.Now.AddHours(2), EndTime = DateTime.Now.AddHours(3) }
            };

            _truckRepositoryMock.Setup(x => x.GetByIdAsync(truck.Id))
                .ReturnsAsync(truck);

            _transitRepositoryMock.Setup(x => x.GetByTruckIdAsync(truck.Id))
                .ReturnsAsync(transitList);

            // Act
            var result = await _transitController.CreateTransit(truck.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CreateTransitViewModel>(viewResult.Model);
            Assert.Equal(truck, model.Truck);
            Assert.Equal(transitList.Count, model.TransitList.Count);
        }

        [Fact]
        public async Task CreateTransit_ReturnsRedirectToActionResult_WhenTransitIsValid()
        {
            // Arrange
            var truck = new Truck
            {
                Id = 1,
                MaxVelocity = 100,
                BreakDuration = 10,
                MinutesUntilBreak = 60
            };

            var model = new CreateTransitViewModel
            {
                Truck = truck,
                Distance = 100,
                StartTime = DateTime.Now,
                TransitList = new List<Transit>
                {
                    new Transit { Id = 1, TruckId = 1, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(1) },
                    new Transit { Id = 2, TruckId = 1, StartTime = DateTime.Now.AddHours(2), EndTime = DateTime.Now.AddHours(3) }
                }
            };

            _truckRepositoryMock.Setup(x => x.GetByIdAsync(truck.Id))
                .ReturnsAsync(truck);
            _transitRepositoryMock.Setup(x => x.GetByTruckIdAsync(truck.Id))
                .ReturnsAsync(model.TransitList);

            // Act
            var result = await _transitController.CreateTransit(model);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("CreateTransit", redirectToActionResult.ActionName);
            Assert.Equal(truck.Id, redirectToActionResult.RouteValues["truckId"]);
            Assert.True(_transitController.TempData.ContainsKey("ErrorMessage"));
        }

        [Fact]
        public async Task CreateTransit_ReturnsRedirectToActionResult_WhenTransitOverlaps()
        {
            // Arrange

            var truck = new Truck
            {
                Id = 1,
                MaxVelocity = 100,
                BreakDuration = 10,
                MinutesUntilBreak = 60
            };

            var savedTransit = new Transit
            {
                Id = 1,
                TruckId = 1,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1)
            };

            var model = new CreateTransitViewModel
            {
                Truck = truck,
                Distance = 100,
                StartTime = DateTime.Now.AddHours(0.5),
                TransitList = new List<Transit>
                {
                    savedTransit
                }
            };

            _truckRepositoryMock.Setup(x => x.GetByIdAsync(truck.Id))
                .ReturnsAsync(truck);

            // Act
            var result = await _transitController.CreateTransit(model);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("CreateTransit", redirectToActionResult.ActionName);
            Assert.Equal(truck.Id, redirectToActionResult.RouteValues["truckId"]);
            Assert.True(_transitController.TempData.ContainsKey("ErrorMessage"));
        }

        [Fact]
        public async Task CreateTransit_ReturnsRedirectToActionResult_WhenModelIsInvalid()
        {
            // Arrange
            var truck = new Truck
            {
                Id = 1,
                MaxVelocity = 100,
                BreakDuration = 10,
                MinutesUntilBreak = 60
            };
            var model = new CreateTransitViewModel
            {
                Truck = truck,
                Distance = 0, // Invalid
                StartTime = DateTime.Now,
                TransitList = new List<Transit>()
            };

            _truckRepositoryMock.Setup(x => x.GetByIdAsync(truck.Id))
                .ReturnsAsync(truck);

            // Act
            var result = await _transitController.CreateTransit(model);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("CreateTransit", redirectToActionResult.ActionName);
            Assert.Equal(truck.Id, redirectToActionResult.RouteValues["truckId"]);
            Assert.True(_transitController.TempData.ContainsKey("ErrorMessage"));
        }
    }

    public class TruckControllerTests
    {
        private readonly Mock<ITruckRepository> _truckRepositoryMock;
        private readonly TruckController _truckController;
        public TruckControllerTests()
        {
            _truckRepositoryMock = new Mock<ITruckRepository>();
            _truckController = new TruckController(_truckRepositoryMock.Object);

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
            _truckController.TempData = tempData;
        }

        [Fact]
        public async Task EditTruck_ReturnsViewResultWithTruck()
        {
            // Arrange
            var truck = new Truck
            {
                Id = 1,
                MaxVelocity = 100,
                BreakDuration = 10,
                MinutesUntilBreak = 60
            };

            _truckRepositoryMock.Setup(x => x.GetByIdAsync(truck.Id))
                .ReturnsAsync(truck);

            // Act
            var result = await _truckController.EditTruck(truck.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Truck>(viewResult.Model);
            Assert.Equal(truck, model);
        }

        [Fact]
        public async Task EditTruck_ReturnsRedirectToActionResult_WhenModelIsValid()
        {
            // Arrange
            var truck = new Truck
            {
                Id = 1,
                MaxVelocity = 100,
                BreakDuration = 10,
                MinutesUntilBreak = 60
            };

            _truckRepositoryMock.Setup(x => x.UpdateAsync(truck))
                .ReturnsAsync(truck);

            // Act
            var result = await _truckController.EditTruck(truck);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<Truck>(viewResult.Model);
            Assert.Equal(truck, model);
            Assert.True(_truckController.TempData.ContainsKey("SuccessMessage"));

        }

        [Fact]
        public async Task EditTruck_ReturnsRedirectToActionResult_WhenModelIsInvalid()
        {
            // Arrange
            var truck = new Truck
            {
                Id = 1,
                MaxVelocity = 0, // Invalid
                BreakDuration = 0, // Invalid
                MinutesUntilBreak = 0 // Invalid
            };

            // Act
            var result = await _truckController.EditTruck(truck);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ListTrucks", redirectToActionResult.ActionName);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
            Assert.True(_truckController.TempData.ContainsKey("ErrorMessage"));
        }

        [Fact]
        public async Task DeleteTruck_ReturnsRedirectToActionResult()
        {
            // Arrange
            int truckId = 1;

            _truckRepositoryMock.Setup(x => x.DeleteAsync(truckId))
                .ReturnsAsync(new OkResult());

            // Act
            var result = await _truckController.DeleteTruck(truckId);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("ListTrucks", redirectToActionResult.ActionName);
            Assert.Equal("Home", redirectToActionResult.ControllerName);
            Assert.True(_truckController.TempData.ContainsKey("SuccessMessage"));
        }
    }
}