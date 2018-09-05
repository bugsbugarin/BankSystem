using Bank.Controllers;
using Bank.Domain.Interface;
using Bank.Models.Transaction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Security.Claims;
using Xunit;

namespace Bank.Controller.Test
{
    public class HomeControllerTest
    {
        private Mock<IAccountManager> _accountManagerMock;
        private Mock<ITransactionManager> _transactionManagerMock;
        private HomeController _homeController;

        public HomeControllerTest()
        {
            _accountManagerMock = new Mock<IAccountManager>();
            _transactionManagerMock = new Mock<ITransactionManager>();
            _homeController = new HomeController(_accountManagerMock.Object, _transactionManagerMock.Object);
        }

        [Fact]
        public void TransactionHistory_Success()
        {
            //Arrange
            bool isSent = true;

            _homeController.ControllerContext.HttpContext = new DefaultHttpContext();

            _homeController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "someAuthTypeName"))
                }
            };

            var dic = new System.Collections.Generic.Dictionary<string, Microsoft.Extensions.Primitives.StringValues>();
            dic.Add("start", "1");
            dic.Add("length", "10");
            dic.Add("draw", "1");
            var collection = new Microsoft.AspNetCore.Http.FormCollection(dic);

            _homeController.Request.Form = collection;

            long accountId = 1;
            int pageSize = 10;
            int pageNumber = 1;

             _transactionManagerMock.Setup(x => x.SearchSentTransaction(accountId, pageSize, pageNumber))
                                   .Returns(new TransactionGridModelList()
                                   {
                                       TotalRows = 2,
                                       Transactions = new System.Collections.Generic.List<TransactionGridModel>()
                                       {
                                           new TransactionGridModel(){ Amount = 1, ReceiverAccountNumber = "12312", TransactionDate = DateTime.Now, TransactionId = "1", TransactionType = "Deposit" },
                                           new TransactionGridModel(){ Amount = 1, ReceiverAccountNumber = "12312", TransactionDate = DateTime.Now, TransactionId = "1", TransactionType = "Withdraw" },
                                       }
                                   });

            //Act
            var result = _homeController.TransactionHistory(isSent);
            JsonResult jsonResult = (JsonResult)result;

            //Assert
            Assert.NotNull(jsonResult.Value);
        }
    }
}
