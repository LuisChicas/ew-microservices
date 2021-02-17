using EasyWallet.Business.Abstractions;
using EasyWallet.Business.Exceptions;
using EasyWalletWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EasyWalletWeb.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {
        private readonly IEntryService _entryService;
        private readonly IStringLocalizer<WalletController> _localizer;        
        
        public WalletController(IEntryService entryService, IStringLocalizer<WalletController> localizer)
        {
            _entryService = entryService;
            _localizer = localizer;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Entry(WalletEntry form)
        {
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