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

        private readonly IOrderManagementService _orderManagementService;
        public OrderController(IOrderManagementService orderManagementService)
        {
            _orderManagementService = orderManagementService;
        }
     

        public async Task<IActionResult> Index()
        {
            var orders = await _orderManagementService.GetAllOrder();
            return View(orders);
        }

        public async Task<IActionResult> SaleOrders()
        {
            var orders = await _orderManagementService.GetAllSaleOrder();
            return View(orders);
        }

        public async Task<IActionResult> PurchaseOrders()
        {
            var orders = await _orderManagementService.GetAllPurchaseOrder();
            return View(orders);
        }

        public async Task<IActionResult> CreateOrder()
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


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderModel model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                    await _orderManagementService.CreateOrderAsync(userId, model.ProductId, model.SaleQuantity, model.UnitPrice, model.TotalAmount, model.OrderType);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return View(model);
        }




        public async Task<IActionResult> UpdateOrder(Guid id)
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

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(UpdateOrderModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id == Guid.Empty)
                        return View(model);
                    await _orderManagementService.UpdateOrderAsync(model.Id, model.ProductId, model.SaleQuantity, model.UnitPrice, model.TotalAmount, model.OrderType);
                    TempData["success"] = "Order updated successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            TempData["error"] = "Error occured";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteOrder(Guid id)
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
        public async Task<IActionResult> DeleteOrder(UpdateProductModel model)
        {
            try
            {
                var order = await _orderManagementService.GetOrderByIdAsync(model.Id);
                if (order == null)
                    throw new Exception("Product not found");

                await _orderManagementService.RemoveOrderAsync(order);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return RedirectToAction("Index");
        }

    }
}
