using Inventory.DataAccess.UnitOfWork;
using Inventory.Presentation.Models;
using Inventory.Presentation.Models.VM;
using Inventory.Service.Features.Services;
using Inventory.Service.Features.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory.Presentation.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductManagementService _productManagementService;
        public ProductController(IProductManagementService productManagementService)
        {
            _productManagementService = productManagementService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productManagementService.GetAllProducts();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _productManagementService.GetAllCategoryNameAsync();

            ViewBag.Categories = categories.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
       
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Name == null)
                        return View(model);
                    await _productManagementService.CreateProductAsync(model.Name, model.Description, model.Price,model.QuantityInStock, model.CategoryId);

                    TempData["success"] = "product created successfully";
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


        //public async Task<IActionResult> Update(Guid id)
        //{
        //    var category = await _categoryManagementService.GetCategoryIdAsync(id);
        //    if (category == null)
        //        throw new Exception("Category not found");

        //    var model = new UpdateCategoryModel();
        //    model.Description = category.Description;
        //    model.Name = category.Name;
        //    model.Id = category.Id;

        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Update(UpdateCategoryModel model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (model.Name == null)
        //                return View(model);
        //            await _categoryManagementService.UpdateCategoryAsync(model.Id, model.Name, model.Description);
        //            TempData["success"] = "Category updated successfully";
        //            return RedirectToAction("Index");
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }
        //    }
        //    TempData["error"] = "Error occured";

        //    return View(model);
        //}

        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    var category = await _categoryManagementService.GetCategoryIdAsync(id);
        //    if (category == null)
        //        throw new Exception("Category not found");

        //    var model = new UpdateCategoryModel();
        //    model.Description = category.Description;
        //    model.Name = category.Name;
        //    model.Id = category.Id;

        //    return View(model);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Delete(UpdateCategoryModel model)
        //{
        //    try
        //    {
        //        var category = await _categoryManagementService.GetCategoryIdAsync(model.Id);
        //        if (category == null)
        //            throw new Exception("Category not found");

        //        await _categoryManagementService.RemoveCategoryAsync(category);
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //    return View();
        //}
    }
}
