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
        private readonly ICategoryManagementService _categoryManagementService;
        public CategoryController(ICategoryManagementService categoryManagementService)
        {
            _categoryManagementService = categoryManagementService;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryManagementService.GetAllCategories();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if(model.Name == null)
                        return View(model);
                    await _categoryManagementService.CreateCategory(model.Name, model.Description);
                    TempData["success"] = "Category created successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            TempData["error"] = "Error occured";

            return View(model);
        }


        public async Task<IActionResult> Update(Guid id)
        {
            var category = await _categoryManagementService.GetCategoryIdAsync(id);
            if (category == null)
                throw new Exception("Category not found");

            var model = new UpdateCategoryModel();
            model.Description = category.Description;
            model.Name = category.Name;
            model.Id = category.Id;

            return View(model);
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
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            TempData["error"] = "Error occured";

            return View(model);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _categoryManagementService.GetCategoryIdAsync(id);
            if (category == null)
                throw new Exception("Category not found");

            var model = new UpdateCategoryModel();
            model.Description = category.Description;
            model.Name = category.Name;
            model.Id = category.Id;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateCategoryModel model)
        {
            try
            {
                var category = await _categoryManagementService.GetCategoryIdAsync(model.Id);
                if (category == null)
                    throw new Exception("Category not found");

                await _categoryManagementService.RemoveCategoryAsync(category);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
    }
}
