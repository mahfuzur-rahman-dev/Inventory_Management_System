using Inventory.DataAccess.Entites;
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


        public async Task<IActionResult> Update(Guid id)
        {
            var product = await _productManagementService.GetProductByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found");

            var model = new UpdateProductModel();
            model.Description = product.Description;
            model.Name = product.Name;
            model.Price = product.Price;
            model.QuantityInStock = product.QuantityInStock;
            model.CategoryId = product.Id;

            var categories = await _productManagementService.GetAllCategoryNameAsync();

            ViewBag.Categories = categories.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });

            ViewBag.SelectedCategory = new SelectListItem
            {
                Text = product.Category.Name,
                Value = product.Category.Id.ToString()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateProductModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Name == null)
                        return View(model);
                    await _productManagementService.UpdateProductAsync(model.Id, model.Name, model.Description,model.Price,model.QuantityInStock,model.CategoryId);
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
            var product = await _productManagementService.GetProductByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found");

            var model = new UpdateProductModel();
            model.Description = product.Description;
            model.Name = product.Name;
            model.Price = product.Price;
            model.QuantityInStock = product.QuantityInStock;
            model.CategoryId = product.Id;
            ViewBag.SelectedCategoryName = product.Category.Name;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateProductModel model)
        {
            try
            {
                var product = await _productManagementService.GetProductByIdAsync(model.Id);
                if (product == null)
                    throw new Exception("Product not found");

                await _productManagementService.RemoveProductAsync(product);
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
