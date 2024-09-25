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
    public class CartController : Controller
    {
        private readonly ICartManagementService _cartManagementService;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public CartController(ICartManagementService cartManagementService, UserManager<ApplicationIdentityUser> userManager, IUnitOfWork unitOfWork)
        {
            _cartManagementService = cartManagementService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }


        public async Task<IActionResult> ViewCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartEntriesOfUser = await _cartManagementService.GetCartByUserIdAsync(Guid.Parse(userId));
            return View(cartEntriesOfUser);
        }

        

        public async Task<IActionResult> AddToCart(int count, Guid productId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var product = await _cartManagementService.GetProductById(productId);
                if (product == null)
                    throw new Exception("Product not found");

                await _cartManagementService.AddToCartAsync(count, product.Id,userId);

                return Json(new { success = true, message = "Product added to cart." });
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var cartEntriesOfUser = await _cartManagementService.GetCartByIdAsync(Guid.Empty);
            return RedirectToAction("ViewCart");
        }

    }
}
