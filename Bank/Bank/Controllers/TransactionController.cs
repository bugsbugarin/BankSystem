using Bank.Models.Transaction;
using Microsoft.AspNetCore.Mvc;
using static Bank.Helper.Enums;
using Bank.Domain.Interface;
using System.Security.Claims;
using Bank.Exception;
using Microsoft.AspNetCore.Authorization;
using Bank.Helper.Attribute;

namespace Bank.Controllers
{
    public class TransactionController : Controller
    {
        ITransactionManager _transactionManager;

        public TransactionController(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        [Authorize]
        [ImportModelStateAttribute]
        public IActionResult Index()
        {
            TransactionRequestViewModel model = new TransactionRequestViewModel();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ExportModelState]
        public IActionResult Transact(TransactionRequestViewModel model)
        {
            model.AccountId = long.Parse(((System.Security.Claims.ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.NameIdentifier).Value);
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.TransactionType == (int)TransactionType.Deposit)
                    {
                        _transactionManager.Deposit(model);
                    }

                    if (model.TransactionType == (int)TransactionType.Withdraw)
                    {
                        _transactionManager.Withdraw(model);
                    }

                    if (model.TransactionType == (int)TransactionType.Transfer)
                    {
                        _transactionManager.Transfer(model);
                    }

                    return RedirectToAction("Index", "Home");
                }
                catch (AppException app)
                {
                    ModelState.AddModelError(string.Empty, app.Message);
                }
            }

            return RedirectToAction("Index", "Transaction");
        }
    }
}
