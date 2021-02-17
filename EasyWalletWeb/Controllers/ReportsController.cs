using EasyWallet.Business.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EasyWalletWeb.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IEntryService _entryService;

        public ReportsController(IReportService reportService, IEntryService entryService)
        {
            _reportService = reportService;
            _entryService = entryService;
        }

        public async Task<IActionResult> History()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var historyReport = await _reportService.GetHistoryReport(userId);
            return View(historyReport);
        }

        [HttpPost]
        public async Task<IActionResult> HistoryDelete(int id)
        {
            await _entryService.DeleteEntry(id);
            return RedirectToAction("History");
        }

        public async Task<IActionResult> Monthly()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var monthlyReport = await _reportService.GetMonthlyReport(userId);
            return View(monthlyReport);
        }

        public async Task<IActionResult> Balance()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var balanceReport = await _reportService.GetBalanceReport(userId);
            return View(balanceReport);
        }
    }
}