using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Bank.Domain.Interface;
using Bank.Exception;
using Bank.Helper;
using Bank.Models.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Bank.Controllers
{
    public class AccountController : Controller
    {
        IAccountManager _accountManager;

        public AccountController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public async Task<IActionResult> Logout(LoginRequestViewModel model)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public IActionResult Register(RegisterRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _accountManager.Register(model);
                    return RedirectToAction("Login", "Account");
                }
                catch (AppException app)
                {
                    ModelState.AddModelError(string.Empty, app.Message);
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    LoginResponseViewModel response = _accountManager.Login(model);

                    if (response != null)
                    {
                        var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier, response.Id.ToString()),
                        new Claim(ClaimTypes.Name, response.LoginName),
                    };

                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties()
                        {
                            IsPersistent = true
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (AppException app)
                {
                    ModelState.AddModelError(string.Empty, app.Message);
                }
            }

            return View();
        }
    }
}
