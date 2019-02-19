using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyWalletWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyWalletWeb.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {
        private readonly DatabaseContext _context;

        public WalletController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToRoute("home");
            }

            return View();
        }
    }
}