using Inventory.Presentation.Models;
using Inventory.Presentation.Models.VM;
using Inventory.Service.Features.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Inventory.Presentation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductManagementService _productManagementService;
        private readonly ICategoryManagementService _categoryManagementService;
        private readonly IOrderManagementService _orderManagementService;

        public HomeController(
            ILogger<HomeController> logger,
            IProductManagementService productManagementService,
            ICategoryManagementService categoryManagementService,
            IOrderManagementService orderManagementService)
        {
            _logger = logger;
            _productManagementService = productManagementService;
            _categoryManagementService = categoryManagementService;
            _orderManagementService = orderManagementService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeIndexVM();

            try
            {
                var totalCategoriesCount = await _categoryManagementService.GetAllCategoryCount();
                var totalProductStockCount = await _productManagementService.GetAllStockProductCount();
                var totalPurchaseOrderCount = await _orderManagementService.GetAllPurchaseOrderCount();
                var totalSaleOrderCount = await _orderManagementService.GetAllSaleOrderCount();

                var totalPurchaseOrderToday = await _orderManagementService.GetAllPurchaseOrderCount(x => x.CreatedDate == DateTime.Today);
                var totalSaleOrderToday = await _orderManagementService.GetAllSaleOrderCount(x => x.CreatedDate == DateTime.Today);

                model.TotalCategories = totalCategoriesCount;
                model.TotalProducts = totalProductStockCount;
                model.TotalPurchaseOrders = totalPurchaseOrderCount;
                model.TotalSaleOrders = totalSaleOrderCount;

                model.TodayPurchaseOrders = totalPurchaseOrderToday;
                model.TodaySaleOrders = totalSaleOrderToday;

                _logger.LogInformation("Index page loaded....");

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to load home index action data";
                _logger.LogInformation("Error occured in home index action");
                _logger.LogError($"Error: {ex}");
                return View(model);
            }


        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
