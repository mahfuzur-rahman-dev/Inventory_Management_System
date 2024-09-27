using Inventory.DataAccess.Entites;
using Inventory.Presentation.Models;
using Inventory.Service.Features.Services.IServices;
using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> PlaceOrder(ViewCartDTO dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            IEnumerable<OrderDetail> orderDetailsEnumerable = await _orderManagementService.GetAllOrderDetailsByUserId(userId);
            List<OrderDetail> orderDetails = orderDetailsEnumerable.ToList();


            if (orderDetails == null || !orderDetails.Any())
            {
                return Json(new { success = false, message = "No order details received." });
            }

            try
            {
                await _orderManagementService.CreateOrderAsync(orderDetails,userId, dto.TotalQuantity, dto.TotalAmount);
                
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
    }
}
