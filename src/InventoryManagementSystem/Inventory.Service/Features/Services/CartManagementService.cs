using Inventory.DataAccess.Entites;
using Inventory.DataAccess.IdentityManager;
using Inventory.DataAccess.UnitOfWork;
using Inventory.Service.Features.Services.IServices;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Features.Services
{
    public class CartManagementService : ICartManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductManagementService _productManagementService;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        public CartManagementService(IUnitOfWork unitOfWork,IProductManagementService productManagementService, UserManager<ApplicationIdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _productManagementService = productManagementService;
            _userManager = userManager;
        }


        public async Task<IEnumerable<Cart>> GetAllCart()
        {
            return await _unitOfWork.Cart.GetAllAsync();
        }


        public async Task AddToCartAsync(int count, Guid productId,Guid userId)
        {
            var product = await GetProductById(productId);

            var existanceObj = await _unitOfWork.Cart.GetAllAsync(x => x.UserId == userId && x.ProductId == productId);
            if(existanceObj.Count == 1 && product is not null)
            {
                var obj = existanceObj.FirstOrDefault();
                obj.TotalQuantity += count;
                obj.TotalAmount = obj.TotalAmount + (product.Price * count);

                await _unitOfWork.Cart.UpdateAsync(obj);
                return;
            }

            var cartObj = new Cart()
            {
                UserId = userId,
                ProductId = productId,
                TotalQuantity = count,
                TotalAmount = (product.Price * count),
            };
            await _unitOfWork.Cart.CreateAsync(cartObj);
            

        }


        public async Task<Cart> GetCartByIdAsync(Guid id)
        {
            return await _unitOfWork.Cart.GetAsync(x=>x.Id == id);
        }

        public async Task RemoveCartAsync(Cart cart)
        {
            await _unitOfWork.Cart.RemoveAsync(cart);
        }

        public async Task UpdateCartAsync(Guid id, string name, string description)
        {
            
            var category = await GetCartByIdAsync(id);
            if (category == null)
                throw new Exception("Category not found");


            //category.Description = description;
            //category.Name = name;
            //await _unitOfWork.Category.UpdateAsync(category);

        }

        public async Task<Product> GetProductById(Guid productId)
        {
            return await _productManagementService.GetProductByIdAsync(productId);
        }

        public async Task<IEnumerable<Cart>> GetCartByUserIdAsync(Guid userId)
        {
            return await _unitOfWork.Cart.GetAllAsync(x => x.UserId == userId, includeProperties:"User,Product");
        }
    }
}
