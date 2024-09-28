using Inventory.DataAccess.Entites;
using Inventory.DataAccess.UnitOfWork;
using Inventory.Presentation.Models;
using Inventory.Presentation.Models.VM;
using Inventory.Service.Features.Services;
using Inventory.Service.Features.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Inventory.Presentation.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductManagementService _productManagementService;
        public ProductController(ILogger<ProductController> logger, IProductManagementService productManagementService)
        {
            _logger = logger;
            _productManagementService = productManagementService;
        }

        public async Task<IActionResult> AllProducts()
        {
            var products = await _productManagementService.GetAllProducts();
            _logger.LogInformation("Product controller all product action loaded");
            return View(products);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _productManagementService.GetProductByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found");

            _logger.LogInformation("Single product detail view page loaded");

            return View(product);
        }


        public async Task<IActionResult> Index()
        {
            var products = await _productManagementService.GetAllProducts();
            _logger.LogInformation("Product controller Index loaded");
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

            _logger.LogInformation("Product Create view page loaded");

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
                    await _productManagementService.CreateProductAsync(model.Name, model.Description, model.CategoryId);

                    TempData["Success"] = "Product created successfully";
                    _logger.LogInformation("New product created....");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Failed to create product";
                    _logger.LogInformation("Error occured in create product post action");
                    _logger.LogError($"Error: {ex}");
                }
            }

            _logger.LogInformation("Error occured for create product model validation failed..");
            TempData["Error"] = "Failed to create product";
            return View(model);
        }


        public async Task<IActionResult> Update(Guid id)
        {
            var product = await _productManagementService.GetProductByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found");

            var model = new UpdateProductModel();
            model.Id = product.Id;
            model.Description = product.Description;
            model.Name = product.Name;
            model.CategoryId = product.CategoryId;

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

            _logger.LogInformation("Product update view page loaded...");

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
                    await _productManagementService.UpdateProductAsync(model.Id, model.Name, model.Description,model.CategoryId);

                    TempData["success"] = "Product updated successfully";
                    _logger.LogInformation("Product update successfully....");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Failed to update product";
                    _logger.LogInformation("Error occured in update product post action");
                    _logger.LogError($"Error: {ex}");
                }
            }
            TempData["Error"] = "Failed to update product";
            _logger.LogInformation("Error occured for update product model validation failed..");

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
            model.CategoryId = product.Id;

            ViewBag.SelectedCategoryName = product.Category.Name;

            _logger.LogInformation("Product delete view page loaded...");

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
                TempData["success"] = "Product deleted successfully";
                _logger.LogInformation("Product deleted successfully....");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to delete product";
                _logger.LogInformation("Error occured in delete product post action");
                _logger.LogError($"Error: {ex}");
            }

            TempData["Error"] = "Failed to delete product";
            _logger.LogInformation("Error occured for delete product model validation failed..");

            return View();
        }
    }
}
