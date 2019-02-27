using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyWalletWeb.Models;
using EasyWalletWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyWalletWeb.Controllers
{
    public class ReportsController : Controller
    {
        private readonly DatabaseContext _context;

        public ReportsController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult History()
        {
            var entriesByMonth = GetEntries().GroupBy(e => new DateTime(e.Date.Year, e.Date.Month, 1));

            var entriesObject = new List<KeyValuePair<DateTime, List<Entry>>>();
            var entryPair = new KeyValuePair<DateTime, List<Entry>>();
            for (int i = 0; i < entriesByMonth.Count(); i++)
            {
                entryPair = new KeyValuePair<DateTime, List<Entry>>(
                    entriesByMonth.ElementAt(i).Key,
                    entriesByMonth.ElementAt(i).ToList()
                );
                entriesObject.Add(entryPair);
            }

            return View(new ReportsHistory { Entries = entriesObject });
        }

        [HttpPost]
        public IActionResult HistoryDelete(int id)
        {
            var entry = _context.Entries.FirstOrDefault(e => e.Id == id);
            if (entry == null || entry.DeletedAt != null)
            {
                return NotFound();
            }

            entry.DeletedAt = DateTime.UtcNow;
            _context.Entries.Update(entry);
            _context.SaveChanges();

            return RedirectToAction("History");
        }

        public IActionResult Monthly()
        {
            var entriesByMonth = GetEntries().GroupBy(e => new DateTime(e.Date.Year, e.Date.Month, 1));
            var months = new List<MonthlyItem>();
            var month = new MonthlyItem();

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User user = _context.Users.Where(u => u.Id == userId).Include(u => u.Categories).First();
            List<Category> categories = user.Categories;

            for(int i = 0; i < entriesByMonth.Count(); i++)
            {
                month = new MonthlyItem();
                month.Month = entriesByMonth.ElementAt(i).Key;

                foreach(var category in categories)
                {
                    decimal sum = GetSumByCategory(entriesByMonth.ElementAt(i).ToList(), category);
                    if (sum > 0)
                    {
                        month.SpendsByCategory.Add(new KeyValuePair<string, decimal>(category.Name, sum));
                    }
                }

                if (month.SpendsByCategory.Count > 0)
                {
                    months.Add(month);
                }
            }

            return View(new ReportsMonthly { Spends = months });
        }

        private List<Entry> GetEntries()
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            User user = _context.Users.Where(u => u.Id == userId).Include(u => u.Categories).First();
            Tag[] tags = _context.Tags.ToArray();
            tags = _context.Tags.Where(t =>
                user.Categories.FirstOrDefault(c => c.Id == t.CategoryId) != null
                && t.DeletedAt == null)
                .Include(t => t.Entries)
                .ToArray();

            var entries = new List<Entry>();
            foreach (var t in tags)
            {
                entries.AddRange(t.Entries);
            }

            return entries
                .Where(e => e.DeletedAt == null)
                .OrderByDescending(e => e.Date)
                .ToList();
        }

        private decimal GetSumByCategory(List<Entry> entries, Category category)
        {
            decimal total = 0;

            for (int i = 0; i < entries.Count; i++)
            {
                if (entries[i].Tag.CategoryId == category.Id)
                {
                    total += entries[i].Amount;
                }
            }

            return total;
        }
    }
}