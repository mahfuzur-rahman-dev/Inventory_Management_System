using Inventory.DataAccess.UnitOfWork;
using Inventory.Presentation.Models;
using Inventory.Presentation.Models.VM;
using Inventory.Service.Features.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Presentation.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryManagementService _categoryManagementService;
        public CategoryController(ILogger<CategoryController> logger, ICategoryManagementService categoryManagementService)
        {
            _logger = logger;
            _categoryManagementService = categoryManagementService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryManagementService.GetAllCategories();
            _logger.LogInformation("Category Index loaded");
            return View(categories);
        }

        public IActionResult Create()
        {
            _logger.LogInformation("Category Create view page loaded");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Name == null)
                        return View(model);
                    await _categoryManagementService.CreateCategory(model.Name, model.Description);

                    TempData["Success"] = "Category created successfully";
                    _logger.LogInformation("New Category created....");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Failed to create category";
                    _logger.LogInformation("Error occured in create category post action");
                    _logger.LogError($"Error: {ex}");
                    return RedirectToAction("Create");
                }
            }
            _logger.LogInformation("Error occured for create category model validation failed..");
            TempData["Error"] = "Failed to create category";

            return View(model);
        }


        public async Task<IActionResult> Update(Guid id)
        {

            var model = new UpdateCategoryModel();

            try
            {
                var category = await _categoryManagementService.GetCategoryIdAsync(id);
                if (category == null)
                {
                    throw new Exception("Category not found");
                }

                model.Description = category.Description;
                model.Name = category.Name;
                model.Id = category.Id;

                _logger.LogInformation("Category update view page loaded...");

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to update category";
                _logger.LogInformation("Error occured in update product action");
                _logger.LogError($"Error: {ex}");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Name == null)
                        return View(model);

                    await _categoryManagementService.UpdateCategoryAsync(model.Id, model.Name, model.Description);
                    TempData["success"] = "Category updated successfully";
                    _logger.LogInformation("Category  update successfully....");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Failed to update category";
                    _logger.LogInformation("Error occured in update category post action");
                    _logger.LogError($"Error: {ex}");

                    return RedirectToAction("Index");
                }
            }
            TempData["Error"] = "Failed to update category";
            _logger.LogInformation("Error occured for update category model validation failed..");

            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var model = new UpdateCategoryModel();
            if (ModelState.IsValid) {
                var category = await _categoryManagementService.GetCategoryIdAsync(id);
                if (category == null)
                    throw new Exception("Category not found");

                model.Description = category.Description;
                model.Name = category.Name;
                model.Id = category.Id;

                _logger.LogInformation("Category delete view page loaded...");
                return View(model);

            }
            TempData["Error"] = "Failed to update category";
            _logger.LogInformation("Error occured for update category model validation failed..");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var category = await _categoryManagementService.GetCategoryIdAsync(model.Id);
                    if (category == null)
                        throw new Exception("Category not found");

                    await _categoryManagementService.RemoveCategoryAsync(category);

                    TempData["success"] = "Category deleted successfully";
                    _logger.LogInformation("Category  deleted successfully....");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Failed to delete category";
                    _logger.LogInformation("Error occured in delete category post action");
                    _logger.LogError($"Error: {ex}");
                    return View(model);
                }
            }

            TempData["Error"] = "Failed to delete category";
            _logger.LogInformation("Error occured for delete category model validation failed..");

            return View(model);
        }
    }
}
