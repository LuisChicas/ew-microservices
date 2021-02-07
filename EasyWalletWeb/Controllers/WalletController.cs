using EasyWallet.Business.Abstractions;
using EasyWallet.Business.Exceptions;
using EasyWalletWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EasyWalletWeb.Controllers
{
    public class WalletController : Controller
    {
        private readonly IEntryService _entryService;
        private readonly IStringLocalizer<WalletController> _localizer;        
        
        public WalletController(IEntryService entryService, IStringLocalizer<WalletController> localizer)
        {
            _entryService = entryService;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToRoute("login");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Entry(WalletEntry form)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToRoute("login");
            }

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                await _entryService.AddEntry(form.Entry, form.Date, userId);
            }
            catch(InvalidEntryException)
            {
                ModelState.AddModelError("Entry", _localizer["ProvideKeywordAndAmount"]);
                return View("Index", form);
            }
            catch(InvalidEntryAmountException)
            {
                ModelState.AddModelError("Entry", _localizer["ProvideValidAmount"]);
                return View("Index", form);
            }

            return View("Index", new WalletEntry { PreviousEntrySaved = true });
        }
    }
}