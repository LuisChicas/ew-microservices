using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyWalletWeb.Infrastructure;
using EasyWalletWeb.Models;
using EasyWalletWeb.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EasyWalletWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IStringLocalizer<AuthController> _localizer;

        public AuthController(DatabaseContext context, IStringLocalizer<AuthController> localizer)
        {
            _context = context;
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
            var user = _context.Users.FirstOrDefault(u => u.Email == form.Email);
            if (user == null)
            {
                Models.User.FakeHash();
                ModelState.AddModelError("Email", _localizer["InvalidEmailPassword"]);
                return View(form);
            }

            if (!user.CheckPassword(form.Password))
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
            if (_context.Users.Any(u => u.Email == form.Email))
            {
                ModelState.AddModelError("Email", _localizer["EmailAlreadyRegistered"]);
                return View(form);
            }

            var user = new User();
            user.Name = form.Name;
            user.Email = form.Email;
            user.SetPassword(form.Password);
            user.CreatedAt = DateTime.UtcNow;

            _context.Users.Add(user);
            _context.SaveChanges();

            await Authenticate(user);

            SaveDefaultData(user.Id);

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

        private void SaveDefaultData(int userId)
        {
            var others = new Category();
            others.Name = _localizer["Others"];
            others.UserId = userId;
            others.CategoryTypeId = Constants.ExpensesCategoryTypeID;
            others.CreatedAt = DateTime.UtcNow;

            var incomes = new Category();
            incomes.Name = _localizer["Incomes"];
            incomes.UserId = userId;
            incomes.CategoryTypeId = Constants.IncomesCategoryTypeID;
            incomes.CreatedAt = DateTime.UtcNow;

            _context.Categories.Add(others);
            _context.Categories.Add(incomes);

            var othersTag = new Tag();
            othersTag.Name = _localizer["Others"];
            othersTag.CategoryId = others.Id;

            var incomeTag = new Tag();
            incomeTag.Name = _localizer["Income"];
            incomeTag.CategoryId = incomes.Id;

            _context.Tags.Add(othersTag);
            _context.Tags.Add(incomeTag);
            _context.SaveChanges();
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToRoute("login");
        }
    }
}