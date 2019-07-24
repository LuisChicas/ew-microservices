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
    public class CategoriesController : Controller
    {
        private readonly DatabaseContext _context;

        public CategoriesController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var categories = _context.Categories.Include(c => c.Tags)
                .Where(c => c.UserId == int.Parse(userId) && c.DeletedAt == null)
                .OrderByDescending(c => c.CreatedAt)
                .ToList();

            foreach (var c in categories)
            {
                c.Tags = c.Tags.Where(t => t.DeletedAt == null).ToList();
            }

            return View(new CategoriesIndex { Categories = categories });
        }

        public IActionResult New() => View("Form", new CategoriesForm { IsNew = true });

        [HttpPost]
        public IActionResult New(CategoriesForm form)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userCategories = _context.Categories.Where(c => c.UserId == userId && c.DeletedAt == null).ToArray();

            var duplicatedCategory = userCategories.FirstOrDefault(c => c.Name == form.Name);
            if (duplicatedCategory != null)
            {
                ModelState.AddModelError("Name", "A category with this name already exists");
                return View(form);
            }

            var duplicatedTag = _context.Tags.FirstOrDefault(t => 
                t.DeletedAt == null &&
                userCategories.Any(c => c.Id == t.CategoryId) && 
                form.Tags.Any(tags => tags.Name == t.Name));

            if (duplicatedTag != null)
            {
                ModelState.AddModelError(
                    "Tags", 
                    string.Format("The tag '{0}' already exists in other category", duplicatedTag.Name)
                );

                form.IsNew = true;
                return View("Form", form);
            }

            var category = new Category();
            category.UserId = userId;
            category.Name = form.Name;
            category.CreatedAt = DateTime.UtcNow;

            _context.Categories.Add(category);
            _context.SaveChanges();

            Tag tag;
            int categoryId = _context.Categories.First(c => 
                c.UserId == userId && 
                c.Name == category.Name && 
                c.DeletedAt == null).Id;

            foreach (var t in form.Tags)
            {
                if (!string.IsNullOrEmpty(t.Name))
                {
                    tag = new Tag();
                    tag.CategoryId = categoryId;
                    tag.Name = t.Name;
                    tag.CreatedAt = DateTime.UtcNow;

                    _context.Tags.Add(tag);
                }
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Include(c => c.Tags).FirstOrDefault(c => c.Id == id);
            if (category == null || category.DeletedAt != null)
            {
                return NotFound();
            }

            return View("Form", new CategoriesForm
            {
                Id = category.Id,
                Name = category.Name,
                Tags = category.Tags,
                IsNew = false
            });
        }

        [HttpPost]
        public IActionResult Edit(CategoriesForm form)
        {
            int id = form.Id;
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null || category.DeletedAt != null)
            {
                return NotFound();
            }

            var duplicatedCategory = _context.Categories.FirstOrDefault(c => 
                c.Id != id && 
                c.UserId == userId && 
                c.Name == form.Name && 
                c.DeletedAt == null);

            if (duplicatedCategory != null)
            {
                ModelState.AddModelError("Name", "A category with this name already exists");
                return View("Form", form);
            }

            var userCategories = _context.Categories.Where(c => c.UserId == userId && c.DeletedAt == null).ToArray();
            var userTags = _context.Tags.Where(t => userCategories.Any(c => c.Id == t.CategoryId) && t.DeletedAt == null).ToArray();

            var duplicatedTag = userTags.FirstOrDefault(t => 
                t.CategoryId != id && 
                form.Tags.Any(tags => tags.Name == t.Name));

            if (duplicatedTag != null)
            {
                ModelState.AddModelError(
                    "Tags",
                    string.Format("The tag '{0}' already exists in other category", duplicatedTag.Name)
                );
                return View("Form", form);
            }

            category.Name = form.Name;

            Tag tag;
            foreach (var t in form.Tags)
            {
                if (!string.IsNullOrEmpty(t.Name) && !userTags.Any(existingTag => existingTag.Name == t.Name))
                {
                    tag = new Tag();
                    tag.CategoryId = id;
                    tag.Name = t.Name;
                    _context.Tags.Add(tag);
                }
            }

            foreach (var t in category.Tags)
            {
                if (!form.Tags.Any(formTag => formTag.Name == t.Name))
                {
                    t.DeletedAt = DateTime.UtcNow;
                    _context.Tags.Update(t);
                }
            }

            _context.Update(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null || category.DeletedAt != null)
            {
                return NotFound();
            }

            category.DeletedAt = DateTime.UtcNow;
            _context.Categories.Update(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}