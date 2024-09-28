using Inventory.DataAccess.Enums;
using Inventory.Presentation.Models;
using Inventory.Service.Features.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Presentation.Controllers
{
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
        public async Task<IActionResult> PurchaseReport(string searchFrom, string searchTo)
        {
            if (string.IsNullOrEmpty(searchFrom) || string.IsNullOrEmpty(searchTo))
            {
                return BadRequest("Both dates are required.");
            }

            // Parse the SearchFrom and SearchTo into DateTime objects
            if (!DateTime.TryParse(searchFrom, out DateTime parsedFrom) ||
                !DateTime.TryParse(searchTo, out DateTime parsedTo))
            {
                return BadRequest("Invalid date range.");
            }

            // Retrieve the reports based on the parsed dates
            var reports = await _orderManagementService.GetOrdersByDateRangeAndType(parsedFrom, parsedTo, OrderType.Purchase.ToString());

            // Return the reports as JSON for the AJAX call
            return Json(reports);
        }





        public IActionResult SaleReport()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SaleReport(CreateReportModel model)
        {
            return View();
        }
    }
}
