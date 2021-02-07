﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyWallet.Business.Abstractions;
using EasyWalletWeb.Infrastructure;
using EasyWalletWeb.Models;
using EasyWalletWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace EasyWalletWeb.Controllers
{    
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IStringLocalizer<CategoriesController> _localizer;

        public CategoriesController(ICategoryService categoryService, IStringLocalizer<CategoriesController> localizer)
        {
            _categoryService = categoryService;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var categories = await _categoryService.GetActiveCategoriesByUser(int.Parse(userId));

            return View(new CategoriesIndex { Categories = categories });
        }

        public IActionResult New() => View("Form", new CategoriesForm { IsNew = true });        

        [HttpPost]
        public async Task<IActionResult> New(CategoriesForm form)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var categories = await _categoryService.GetActiveCategoriesByUser(userId);

            var duplicatedCategory = categories.FirstOrDefault(c => c.Name == form.Name);
            if (duplicatedCategory != null)
            {
                ModelState.AddModelError("Name", _localizer["NameAlreadyExists"]);
                form.IsNew = true;
                return View("Form", form);
            }

            var duplicatedTag = categories
                .SelectMany(c => c.Tags)
                .FirstOrDefault(t => form.Tags.Any(formTag => formTag.Name == t.Name));

            if (duplicatedTag != null)
            {
                string message = string.Format(
                    "{0}{1}{2}",
                    _localizer["KeywordAlreadyExists1"],
                    duplicatedTag.Name,
                    _localizer["KeywordAlreadyExists2"]);

                ModelState.AddModelError("Tags", message);

                form.IsNew = true;
                return View("Form", form);
            }

            await _categoryService.CreateCategory(userId, form.Name, form.Tags);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetActiveCategoryById(id);
            if (category == null)
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
        public async Task<IActionResult> Edit(CategoriesForm form)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var categories = await _categoryService.GetActiveCategoriesByUser(userId);

            if (!categories.Any(c => c.Id == form.Id))
            {
                return NotFound();
            }

            if (categories.Any(c => c.Id != form.Id && c.Name == form.Name))
            {
                ModelState.AddModelError("Name", _localizer["NameAlreadyExists"]);
                return View("Form", form);
            }

            var tags = categories.SelectMany(c => c.Tags);

            var duplicatedTag = tags.FirstOrDefault(t => form.Tags.Any(formTag => formTag.Name == t.Name) && t.CategoryId != form.Id);

            if (duplicatedTag != null)
            {
                string message = string.Format(
                    "{0}{1}{2}",
                    _localizer["KeywordAlreadyExists1"],
                    duplicatedTag.Name,
                    _localizer["KeywordAlreadyExists2"]);

                ModelState.AddModelError("Tags", message);
                return View("Form", form);
            }

            await _categoryService.UpdateCategory(form.Id, form.Name, form.Tags);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetActiveCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            await _categoryService.DeleteCategory(id);

            return RedirectToAction("Index");
        }
    }
}