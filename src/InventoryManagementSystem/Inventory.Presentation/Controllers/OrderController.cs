using Inventory.DataAccess.Entites;
using Inventory.DataAccess.Enums;
using Inventory.Presentation.Models;
using Inventory.Service.Features.Services;
using Inventory.Service.Features.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Inventory.Presentation.Controllers
{
    public class OrderController : Controller
    {

        private readonly IOrderManagementService _orderManagementService;
        public OrderController(IOrderManagementService orderManagementService)
        {
            _orderManagementService = orderManagementService;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(int totalQuantity, Guid productId, string orderType)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            try
            {
                //await _orderManagementService.CreateOrderAsync(userId,TotalQuantity, dtoTotalAmount);
                
                return Json(new { success = true, message = "Order placed successfully!" });
            }
            catch (Exception ex)
            {
                // Log exception and return an error response
                return Json(new { success = false, message = "An error occurred while placing the order: " + ex.Message });
            }
        }

        public async Task<IActionResult> Orders()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var orders = await _orderManagementService.GetAllOrder();

            return View(orders);
        }

        public async Task<IActionResult> CreateSaleOrder()
        {
            var products = await _orderManagementService.GetAllProductNameAsync();

            ViewBag.Products = products.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSaleOrder(CreateSaleOrderModel model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                    await _orderManagementService.CreateSaleOrder(userId,model.ProductId,model.SaleQuantity, model.UnitPrice,model.TotalAmount);
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
