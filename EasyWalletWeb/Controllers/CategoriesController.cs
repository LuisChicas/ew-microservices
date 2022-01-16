using EasyWallet.Business.Abstractions;
using EasyWallet.Business.Clients.Exceptions;
using EasyWalletWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EasyWalletWeb.Controllers
{
    [Authorize]
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

        public IActionResult New() => View("Form", new CategoryForm { IsNew = true });        

        [HttpPost]
        public async Task<IActionResult> New(CategoryForm form)
        {
            int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                await _categoryService.CreateCategory(userId, form.Name, form.Tags);
            }
            catch (DuplicatedCategoryNameException)
            {
                ModelState.AddModelError("Name", _localizer["NameAlreadyExists"]);
                form.IsNew = true;
                return View("Form", form);
            }
            catch (DuplicatedKeywordNameException e)
            {
                string message = string.Format(
                    "{0}{1}{2}",
                    _localizer["KeywordAlreadyExists1"],
                    e.Data["keywordName"],
                    _localizer["KeywordAlreadyExists2"]);

                ModelState.AddModelError("Tags", message);

                form.IsNew = true;
                return View("Form", form);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetActiveCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View("Form", new CategoryForm
            {
                Id = category.Id,
                Name = category.Name,
                Tags = category.Tags,
                IsNew = false
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryForm form)
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