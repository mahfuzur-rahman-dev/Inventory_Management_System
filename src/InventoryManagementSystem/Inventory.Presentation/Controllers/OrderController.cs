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

        public async Task<IActionResult> CreatePurchaseOrder()
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
                Value = o.ToString(),
                Selected = o == OrderType.Purchase
            }).ToList();


            _logger.LogInformation("Order Create view page loaded");

            return View();
        }


        public async Task<IActionResult> CreateSaleOrder()
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
                Value = o.ToString(),
                Selected = o == OrderType.Sale
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

                    await _orderManagementService.CreateOrderAsync(userId, model.ProductId, model.Quantity, model.UnitPrice, model.TotalAmount, model.OrderType);
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




        #region Update purchase order ------------

        public async Task<IActionResult> UpdatePurchaseOrder(Guid id)
        {
            var model = new UpdateOrderModel();

            try
            {
                var order = await _orderManagementService.GetOrderByIdAsync(id);
                if (order == null || id == Guid.Empty)
                    throw new Exception("Order not found");

                model.Id = order.Id;
                model.ProductId = order.ProductId;
                model.Quantity = order.TotalQuantity;
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
                   Value = o.ToString(),
                   Selected = o == OrderType.Sale
               }).ToList();

                _logger.LogInformation("Order update view page loaded...");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to update order";
                _logger.LogInformation("Error occured in update purchase order view model action");
                _logger.LogError($"Error: {ex}");

                return View(model);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePurchaseOrder(UpdateOrderModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id == Guid.Empty)
                        return View(model);
                    await _orderManagementService.UpdateOrderAsync(model.Id, model.ProductId, model.Quantity, model.UnitPrice, model.TotalAmount, model.OrderType);

                    TempData["success"] = "Order updated successfully";
                    _logger.LogInformation("Order update successfully....");

                    return RedirectToAction("PurchaseOrders");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Failed to update order";
                    _logger.LogInformation("Error occured in update order post action");
                    _logger.LogError($"Error: {ex}");

                    return RedirectToAction("UpdatePurchaseOrder", new { id = model.Id });
                }
            }
            TempData["Error"] = "Failed to update order";
            _logger.LogInformation("Error occured for update order model validation failed..");

            return RedirectToAction("UpdatePurchaseOrder", new { id = model.Id });
        }


        #endregion



        #region Update sales order --------------

        public async Task<IActionResult> UpdateSaleOrder(Guid id)
        {

            var model = new UpdateSaleOrderModel();

            try
            {

                var order = await _orderManagementService.GetOrderByIdAsync(id);
                if (order == null || id == Guid.Empty)
                    throw new Exception("Order not found");

                model.Id = order.Id;
                model.ProductId = order.ProductId;
                model.Quantity = order.TotalQuantity;
                model.UnitPrice = order.UnitPrice;
                model.TotalAmount = order.TotalAmount;
                model.OrderType = order.OrderType;
                model.Product = order.Product;


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
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to update sale order";
                _logger.LogInformation("Error occured in update sale order post action");
                _logger.LogError($"Error: {ex}");
                return View(model);

            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateSaleOrder(UpdateSaleOrderModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.Id == Guid.Empty)
                        return View(model);
                    await _orderManagementService.UpdateOrderAsync(model.Id, model.ProductId, model.Quantity, model.UnitPrice, model.TotalAmount, model.OrderType);

                    TempData["success"] = "Order updated successfully";
                    _logger.LogInformation("Order update successfully....");

                    return RedirectToAction("SaleOrders");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Failed to update order";
                    _logger.LogInformation("Error occured in update order post action");
                    _logger.LogError($"Error: {ex}");

                    return RedirectToAction("UpdateSaleOrder", new { id = model.Id });

                }
            }
            TempData["Error"] = "Failed to update order";
            _logger.LogInformation("Error occured for update order model validation failed..");

            return RedirectToAction("UpdateSaleOrder", new { id = model.Id });

        }

        #endregion



        #region Delete order -------------------

        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var model = new UpdateOrderModel();

            try
            {
                var order = await _orderManagementService.GetOrderByIdAsync(id);
                if (order == null || id == Guid.Empty)
                    throw new Exception("Order not found");

                model.Id = order.Id;
                model.ProductId = order.ProductId;
                model.Quantity = order.TotalQuantity;
                model.UnitPrice = order.UnitPrice;
                model.TotalAmount = order.TotalAmount;
                model.OrderType = order.OrderType;
                model.Product = order.Product;

                return View(model);

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed to delete order";
                _logger.LogInformation("Error occured in delete order view page action");
                _logger.LogError($"Error: {ex}");

                return View(model);

            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(UpdateOrderModel model)
        {
            if (ModelState.IsValid)
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
                    return RedirectToAction("DeleteOrder", new { id = model.Id });
                }
            }

            TempData["Error"] = "Failed to delete order";
            _logger.LogInformation("Error occured for delete order model validation failed..");

            return RedirectToAction("DeleteOrder", new { id = model.Id });
        }

        #endregion


        [HttpGet]
        public async Task<IActionResult> GetProductInfo(string productId)
        {
            var product = await _orderManagementService.GetProductAsync(Guid.Parse(productId));
            if (product == null)
            {
                return Json(new { availableStock = 0, unitPrice = 0 });
            }
            return Json(new { availableStock = product.QuantityInStock, unitPrice = product.Price });
        }


    }
}
