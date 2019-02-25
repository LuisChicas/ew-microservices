using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyWalletWeb.Models;
using EasyWalletWeb.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace EasyWalletWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly DatabaseContext _context;

        public AuthController(DatabaseContext context)
        {
            _context = context;
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
                ModelState.AddModelError("Email", "Invalid email or password");
                return View(form);
            }

            if (!user.CheckPassword(form.Password))
            {
                ModelState.AddModelError("Email", "Invalid email or password");
                return View(form);
            }

            await Authenticate(user);

            return RedirectToRoute("wallet");
        }

        public IActionResult Signup()
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
                ModelState.AddModelError("Email", "This email is already registered");
                return View(form);
            }

            var user = new User();
            user.Name = form.Name;
            user.Email = form.Email;
            user.SetPassword(form.Password);

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
            others.Name = "Others";
            others.UserId = userId;
            _context.Categories.Add(others);
            _context.SaveChanges();

            var tag = new Tag();
            tag.Name = "Others";
            tag.CategoryId = others.Id;
            _context.Tags.Add(tag);
            _context.SaveChanges();
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToRoute("home");
        }
    }
}