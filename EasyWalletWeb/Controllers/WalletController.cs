using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyWalletWeb.Models;
using EasyWalletWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EasyWalletWeb.Controllers
{
    //[Authorize]
    public class WalletController : Controller
    {
        private readonly DatabaseContext _context;

        private const string InvalidEntryFeedback = "Please provide a tag name and an amount of money spent, separated by a space. I.e.: \"Super market $20\"";
        private const string InvalidAmountFeedback  = "Please provide a valid amount of money spent";

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

        [HttpPost]
        public IActionResult Entry(WalletEntry form)
        {
            string entryText = form.Entry;
            if (string.IsNullOrEmpty(entryText))
            {
                ModelState.AddModelError("Entry", InvalidEntryFeedback);
                return View("Index", form);
            }

            string[] entryParts = entryText.Split(' ');
            if (entryParts.Length < 2)
            {
                ModelState.AddModelError("Entry", InvalidEntryFeedback);
                return View("Index", form);
            }

            string amountString = entryParts[entryParts.Length - 1];
            string keyword = entryText.Substring(0, entryText.Length - amountString.Length).Trim();
            if (amountString.Contains("$"))
            {
                amountString = amountString.Remove(0, 1);
            }
            decimal amount;
            if (!decimal.TryParse(amountString, out amount))
            {
                ModelState.AddModelError("Entry", InvalidAmountFeedback);
                return View("Index", form); 
            }
            amount = amount < 0 ? -amount : amount;

            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            Category[] categories = _context.Categories
                .Where(c => c.UserId == userId && c.DeletedAt == null)
                .Include(c => c.Tags)
                .ToArray();

            int tagId = default(int);
            foreach (Category c in categories)
            {
                foreach (var t in c.Tags)
                {
                    if (keyword.ToLower() == t.Name.ToLower())
                    {
                        tagId = t.Id;
                        break;
                    }
                }
            }

            if (tagId == default(int))
            {
                var othersCategory = categories.First(c => c.Name == "Others");
                var tag = new Tag();
                tag.Name = keyword;
                tag.CategoryId = othersCategory.Id;
                _context.Tags.Add(tag);
                _context.SaveChanges();

                tagId = _context.Tags.First(t => t.Name == keyword).Id;
            }

            DateTime today = DateTime.Now;
            var date = new DateTime(
                form.Date.Year, 
                form.Date.Month, 
                form.Date.Day, 
                today.Hour, 
                today.Minute, 
                today.Second);

            var entry = new Entry
            {
                Amount = amount,
                TagId = tagId,
                Date = date
            };

            _context.Entries.Add(entry);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}