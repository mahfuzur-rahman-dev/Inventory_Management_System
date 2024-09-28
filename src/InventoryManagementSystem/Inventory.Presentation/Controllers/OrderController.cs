using Inventory.DataAccess.Entites;
using Inventory.DataAccess.Enums;
using Inventory.Presentation.Models;
using Inventory.Service.Features.Services;
using Inventory.Service.Features.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Inventory.Presentation.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {

        private readonly ILogger<OrderController> _logger;
        private readonly IOrderManagementService _orderManagementService;
        public OrderController(ILogger<OrderController> logger, IOrderManagementService orderManagementService)
        {
            _logger = logger;
            _orderManagementService = orderManagementService;
        }
     

        public async Task<IActionResult> Index()
        {
            var orders = await _orderManagementService.GetAllOrder();
            _logger.LogInformation("Order controller Index loaded");
            return View(orders);
        }

        public async Task<IActionResult> SaleOrders()
        {
            var orders = await _orderManagementService.GetAllSaleOrder();
            _logger.LogInformation("Order controller Sale Orders  loaded");
            return View(orders);
        }

        public async Task<IActionResult> PurchaseOrders()
        {
            var orders = await _orderManagementService.GetAllPurchaseOrder();
            _logger.LogInformation("Order controller purchase orders loaded");
            return View(orders);
        }

        public async Task<IActionResult> Create()
        {
            var products = await _orderManagementService.GetAllProductNameAsync();

            ViewBag.Products = products.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });


            ViewBag.OrderTypes = Enum.GetValues(typeof(OrderType))
            .Cast<OrderType>()
            .Select(o => new SelectListItem
            {
                Text = o.ToString(),
                Value = o.ToString()
            }).ToList();

            _logger.LogInformation("Order Create view page loaded");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderModel model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                    await _orderManagementService.CreateOrderAsync(userId, model.ProductId, model.SaleQuantity, model.UnitPrice, model.TotalAmount, model.OrderType);
                    TempData["Success"] = "Order created successfully";
                    _logger.LogInformation("New order created....");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Failed to create order";
                    _logger.LogInformation("Error occured in create order post action");
                    _logger.LogError($"Error: {ex}");
                }
            }
            _logger.LogInformation("Error occured for create order model validation failed..");
            TempData["Error"] = "Failed to create order";
            return View(model);
        }




        public async Task<IActionResult> Update(Guid id)
        {
            var order = await _orderManagementService.GetOrderByIdAsync(id);
            if (order == null || id  == Guid.Empty)
                throw new Exception("Order not found");

            var model = new UpdateOrderModel();
            model.Id = order.Id;
            model.ProductId = order.ProductId;
            model.SaleQuantity = order.TotalQuantity;
            model.UnitPrice = order.UnitPrice;
            model.TotalAmount = order.TotalAmount;
            model.OrderType = order.OrderType;


            var products = await _orderManagementService.GetAllProductNameAsync();

            ViewBag.Products = products.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });


            ViewBag.OrderTypes = Enum.GetValues(typeof(OrderType))
            .Cast<OrderType>()
            .Select(o => new SelectListItem
            {
                Text = o.ToString(),
                Value = o.ToString()
            }).ToList();

            _logger.LogInformation("Order update view page loaded...");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateOrderModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id == Guid.Empty)
                        return View(model);
                    await _orderManagementService.UpdateOrderAsync(model.Id, model.ProductId, model.SaleQuantity, model.UnitPrice, model.TotalAmount, model.OrderType);

                    TempData["success"] = "Order updated successfully";
                    _logger.LogInformation("Order update successfully....");

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Failed to update order";
                    _logger.LogInformation("Error occured in update order post action");
                    _logger.LogError($"Error: {ex}");
                }
            }
            TempData["Error"] = "Failed to update order";
            _logger.LogInformation("Error occured for update order model validation failed..");

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var order = await _orderManagementService.GetOrderByIdAsync(id);
            if (order == null || id == Guid.Empty)
                throw new Exception("Order not found");

            var model = new UpdateOrderModel();
            model.Id = order.Id;
            model.ProductId = order.ProductId;
            model.SaleQuantity = order.TotalQuantity;
            model.UnitPrice = order.UnitPrice;
            model.TotalAmount = order.TotalAmount;
            model.OrderType = order.OrderType;


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UpdateProductModel model)
        {
            try
            {
                var order = await _orderManagementService.GetOrderByIdAsync(model.Id);
                if (order == null)
                    throw new Exception("Product not found");

                await _orderManagementService.RemoveOrderAsync(order);

                TempData["success"] = "Order deleted successfully";
                _logger.LogInformation("Order deleted successfully....");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to delete order";
                _logger.LogInformation("Error occured in delete order post action");
                _logger.LogError($"Error: {ex}");
            }
            TempData["Error"] = "Failed to delete order";
            _logger.LogInformation("Error occured for delete order model validation failed..");
            return RedirectToAction("Index");
        }

    }
}
