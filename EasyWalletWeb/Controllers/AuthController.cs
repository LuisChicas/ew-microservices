using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyWalletWeb.Models;
using EasyWalletWeb.ViewModels;
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
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup(AuthSignup form)
        {
            if (_context.Users.Any(u => u.Email == form.Email))
            {
                ModelState.AddModelError("Email", "This email is already registered");
                return View(form);
            }

            var user = new User();
            user.Name = form.Name;
            user.Email = form.Email;
            user.PasswordHash = form.Password;

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToRoute("wallet");
        }
    }
}