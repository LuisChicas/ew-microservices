using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EasyWalletWeb.Controllers
{
    public class WalletController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}