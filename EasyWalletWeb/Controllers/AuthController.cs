using EasyWallet.Business.Abstractions;
using EasyWallet.Business.Models;
using EasyWalletWeb.Infrastructure;
using EasyWalletWeb.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EasyWalletWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        private readonly IStringLocalizer<AuthController> _localizer;

        public AuthController(IUserService userService, IStringLocalizer<AuthController> localizer)
        {
            _userService = userService;
            _localizer = localizer;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToRoute("wallet");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(AuthLogin form)
        {
            var user = await _userService.GetUserByEmailAsync(form.Email);
            if (user == null)
            {
                _userService.FakeHash();
                ModelState.AddModelError("Email", _localizer["InvalidEmailPassword"]);
                return View(form);
            }

            if (!_userService.VerifyPassword(form.Password, user))
            {
                ModelState.AddModelError("Email", _localizer["InvalidEmailPassword"]);
                return View(form);
            }

            await Authenticate(user);

            return RedirectToRoute("wallet");
        }

        public ActionResult Signup()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToRoute("wallet");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Signup(AuthSignup form)
        {
            if (await _userService.EmailExistsAsync(form.Email))
            {
                ModelState.AddModelError("Email", _localizer["EmailAlreadyRegistered"]);
                return View(form);
            }

            var user = await _userService.CreateUser(form.Email, form.Password, form.Name);

            await Authenticate(user);

            SetInstructionsCookies();

            return RedirectToRoute("wallet");
        }

        private async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private void SetInstructionsCookies()
        {
            var instructions = new string[] {
                Constants.InstructionNameWelcome,
                Constants.InstructionNameNewCategory,
                Constants.InstructionNameCategories,
                Constants.InstructionNameBalance,
                Constants.InstructionNameMonthly,
                Constants.InstructionNameHistory
            };

            for (int i = 0; i < instructions.Length; i++)
            {
                Response.Cookies.Append(
                    Constants.InstructionsNamePrefix + instructions[i],
                    string.Empty,
                    new Microsoft.AspNetCore.Http.CookieOptions { IsEssential = true }
                );
            }
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToRoute("login");
        }
    }
}