using Bank.Controllers;
using Bank.Domain.Interface;
using Bank.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Bank.Controller.Test
{
    public class AccountControllerTest
    {
        private Mock<IAccountManager> _accountManagerMock;
        private AccountController _accountController;

        public AccountControllerTest()
        {
            _accountManagerMock = new Mock<IAccountManager>();
            _accountController = new AccountController(_accountManagerMock.Object);
            var authenticationServiceMock = new Mock<IAuthenticationService>();
            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            var tempDataDictionary = new Mock<ITempDataDictionaryFactory>();
            authenticationServiceMock
                .Setup(a => a.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(s => s.GetService(typeof(IUrlHelperFactory)))
                .Returns(urlHelperFactory.Object);

            serviceProviderMock
                .Setup(s => s.GetService(typeof(IAuthenticationService)))
                .Returns(authenticationServiceMock.Object);

            serviceProviderMock
                .Setup(s => s.GetService(typeof(ITempDataDictionaryFactory)))
                .Returns(tempDataDictionary.Object);

            _accountController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
                {
                    RequestServices = serviceProviderMock.Object,
                    
                }
            };
        }

        [Fact]
        public void Register_RegisterModelError()
        {
            //Arrange
            var mockRegisterRequest = new RegisterRequestViewModel();
            _accountController.ModelState.AddModelError("LoginName", "Username field is required");
            _accountController.ModelState.AddModelError("Password", "Password field is required");
            //Act
            var result = _accountController.Register(mockRegisterRequest);


            //Assert
            Assert.True(((ViewResult)result).ViewData.ModelState["LoginName"].Errors.Count > 0);
            Assert.True(((ViewResult)result).ViewData.ModelState["Password"].Errors.Count > 0);
        }

        [Fact]
        public void Register_RegisterSuccess()
        {
            //Arrange
            var mockRegisterRequest = new RegisterRequestViewModel()
            {
                Password = "MockPassword",
                Balance = 1,
                ConfirmPassword = "MockPassword",
                LoginName = "MockLoginName"

            };

            // Act
            var result = _accountController.Register(mockRegisterRequest);
            var redirectResult = (RedirectToActionResult)result;

            //Assert
            Assert.True(redirectResult.ControllerName == "Account");
            Assert.True(redirectResult.ActionName == "Login");
        }

        [Fact]
        public async void Login_LoginModelError()
        {
            //Arrange
            var mockLoginRequest = new LoginRequestViewModel();
            _accountController.ModelState.AddModelError("LoginName", "Username field is required");
            _accountController.ModelState.AddModelError("Password", "Password field is required");
            //Act
            var result = await _accountController.Login(mockLoginRequest);


            //Assert
            Assert.True(((ViewResult)result).ViewData.ModelState["LoginName"].Errors.Count > 0);
            Assert.True(((ViewResult)result).ViewData.ModelState["Password"].Errors.Count > 0);
        }

        [Fact]
        public async void Login_Success()
        {
            //Arrange
            var mockLoginRequest = new LoginRequestViewModel()
            {
                Password = "MockPassword",
                LoginName = "MockLoginName"

            };

            _accountManagerMock.Setup(x => x.Login(mockLoginRequest))
                               .Returns(new LoginResponseViewModel() { AccountNumber = "1234", Id = 1, LoginName = "MockLoginName" });
            // Act
            var result = await _accountController.Login(mockLoginRequest);
            var redirectResult = (RedirectToActionResult)result;

            //Assert
            Assert.True(redirectResult.ControllerName == "Home");
            Assert.True(redirectResult.ActionName == "Index");
        }

    }
}
