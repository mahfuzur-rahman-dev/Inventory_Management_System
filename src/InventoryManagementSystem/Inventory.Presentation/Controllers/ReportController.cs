using Inventory.DataAccess.Entites;
using Inventory.DataAccess.Enums;
using Inventory.Presentation.Models;
using Inventory.Presentation.Models.VM;
using Inventory.Service.Features.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Inventory.Presentation.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly ILogger<ReportController> _logger;
        private readonly IOrderManagementService _orderManagementService;
        public ReportController(ILogger<ReportController> logger, IOrderManagementService orderManagementService)
        {
            _logger = logger;
            _orderManagementService = orderManagementService;
        }

        public IActionResult PurchaseReport()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> PurchaseReport(CreateReportModel model)
        {

            var viewModel = new ReportViewModel { CreateReportModel = model };

            if (ModelState.IsValid)
            {
                try
                {
                    var reports = await _orderManagementService.GetOrdersByDateRangeAndType(model.SearchFrom.Date, model.SearchTo.Date, OrderType.Purchase.ToString());
                    viewModel.Reports = reports; 
                    viewModel.ReportType = OrderType.Purchase.ToString();
                    viewModel.SearchFrom = model.SearchFrom;
                    viewModel.SearchTo = model.SearchTo;
                    _logger.LogInformation("Purchase report loaded...");

                    return View("DisplayReport", viewModel); 
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Failed to generate purchase report";
                    _logger.LogInformation("Error occured in purchase report post action");
                    _logger.LogError($"Error: {ex}");
                    return View(model);
                }
            }
            TempData["Error"] = "Failed to load purchase report";
            _logger.LogInformation("Error occured in purchase report post action model validation");

            return View(model);
        }

        public IActionResult DisplayReports(ReportViewModel model)
        {
            _logger.LogInformation("Display report ............");
            TempData["Success"] = "Report generate success...!";
            return View(model); 
        }

        public IActionResult SaleReport()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SaleReport(CreateReportModel model)
        {

            var viewModel = new ReportViewModel { CreateReportModel = model };

            if (ModelState.IsValid)
            {
                try
                {
                    var reports = await _orderManagementService.GetOrdersByDateRangeAndType(model.SearchFrom.Date, model.SearchTo.Date, OrderType.Sale.ToString());
                    viewModel.Reports = reports; 
                    viewModel.ReportType = OrderType.Sale.ToString();

                    _logger.LogInformation("Sale report loaded...");

                    return View("DisplayReport", viewModel); 
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Failed to generate sale report";
                    _logger.LogInformation("Error occured in purchase report post action");
                    _logger.LogError($"Error: {ex}");
                    return View(model);
                }
            }
            TempData["Error"] = "Failed to load sale report";
            _logger.LogInformation("Error occured in sale report post action model validation");
            return View(model);
        }       
    }
}
