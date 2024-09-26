using Inventory.DataAccess.Entites;
using Inventory.DataAccess.IdentityManager;
using Inventory.DataAccess.UnitOfWork;
using Inventory.Presentation.Models;
using Inventory.Presentation.Models.VM;
using Inventory.Service.Features.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Inventory.Presentation.Controllers
{
    public class OrderDetailController : Controller
    {
        private readonly IOrderDetailManagementService _orderDetailManagementService;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public OrderDetailController(IOrderDetailManagementService orderDetailManagementService, UserManager<ApplicationIdentityUser> userManager, IUnitOfWork unitOfWork)
        {
            _orderDetailManagementService = orderDetailManagementService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }


        public async Task<IActionResult> ViewCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartEntriesOfUser = await _orderDetailManagementService.GetOrderDetailByUserIdAsync(Guid.Parse(userId));
            return View(cartEntriesOfUser);
        }

        

        public async Task<IActionResult> AddToOrderDetail(int count, Guid productId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var product = await _orderDetailManagementService.GetProductById(productId);
                if (product == null)
                    throw new Exception("Product not found");

                await _orderDetailManagementService.AddToOrderDetailAsync(count, product.Id,userId);

                return Json(new { success = true, message = "Product added to cart." });
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var cartEntriesOfUser = await _orderDetailManagementService.GetOrderDetailByIdAsync(Guid.Empty);
            return RedirectToAction("ViewOrderDetail");
        }

    }
}
