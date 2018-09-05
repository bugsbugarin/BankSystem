using Bank.Controllers;
using Bank.Domain.Interface;
using Bank.Models.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace Bank.Controller.Test
{
    public class TransactionControllerTest
    {
        private Mock<ITransactionManager> _transactionManagerMock;
        private TransactionController _transactionController;

        public TransactionControllerTest()
        {
            _transactionManagerMock = new Mock<ITransactionManager>();
            _transactionController = new TransactionController(_transactionManagerMock.Object);
        }

        [Fact]
        public void Transact_Sucess()
        {
            //Arrange
            _transactionController.ControllerContext.HttpContext = new DefaultHttpContext();

            _transactionController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "someAuthTypeName"))
                }
            };

            TransactionRequestViewModel request = new TransactionRequestViewModel()
            {
                Amount = 10,
                TransactionType =1
            };
            
            _transactionManagerMock.Setup(x => x.Deposit(request));

            //Act
            var result = _transactionController.Transact(request);
            var redirectResult = (RedirectToActionResult)result;

            //Assert
            Assert.True(redirectResult.ControllerName == "Home");
            Assert.True(redirectResult.ActionName == "Index");
        }
    }
}
