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
        private readonly IOrderManagementService _orderManagementService;
        public ReportController(IOrderManagementService orderManagementService)
        {
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
                    viewModel.Reports = reports; // Assign reports to ViewModel
                    viewModel.ReportType = OrderType.Purchase.ToString();
                    return View("DisplayReport", viewModel); // Return to the same action or different one with ViewModel
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return View(model);
        }


        public IActionResult DisplayReports(ReportViewModel model)
        {
            return View(model); // Pass the ViewModel to the view
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
                    viewModel.Reports = reports; // Assign reports to ViewModel
                    viewModel.ReportType = OrderType.Sale.ToString();
                    return View("DisplayReport", viewModel); // Return to the same action or different one with ViewModel
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return View(model);
        }


        
    }
}
