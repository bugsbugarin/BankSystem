using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Bank.Models;
using Microsoft.AspNetCore.Authorization;
using Bank.Models.Transaction;
using System.Security.Claims;
using Bank.Domain.Interface;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Bank.Controllers
{
    public class HomeController : Controller
    {
        IAccountManager _accountManager;
        ITransactionManager _transactionManager;
        
        public HomeController(IAccountManager accountManager, ITransactionManager transactionManager)
        {
            _accountManager = accountManager;
            _transactionManager = transactionManager;
        }
        [Authorize]
        public IActionResult Index()
        {
            long accountId = long.Parse(((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);

            var response = _accountManager.GetById(accountId);

            return View(response);
        }

        [HttpPost]
        public IActionResult TransactionHistory(bool isSent)
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();

            long accountId = long.Parse(((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);

            var start = int.Parse(Request.Form["start"].FirstOrDefault());
            var pageSize = int.Parse(Request.Form["length"].FirstOrDefault());
           
            var pageNumber = start / pageSize + 1;
            
            var transactionData = new TransactionGridModelList();

            if(isSent)
            {
                transactionData = _transactionManager.SearchSentTransaction(accountId, pageSize, pageNumber);
            }
            else
            {
                transactionData = _transactionManager.SearchReceivedTransaction(accountId, pageSize, pageNumber);
            }
            
            //Returning Json Data  
            return Json(new { draw = draw, recordsFiltered = transactionData.TotalRows, recordsTotal = transactionData.TotalRows, data = transactionData.Transactions });

        }
    }
}