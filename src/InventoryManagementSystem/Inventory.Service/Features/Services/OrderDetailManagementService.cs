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
    public class OrderDetailManagementService : IOrderDetailManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductManagementService _productManagementService;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        public OrderDetailManagementService(IUnitOfWork unitOfWork,IProductManagementService productManagementService, UserManager<ApplicationIdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _productManagementService = productManagementService;
            _userManager = userManager;
        }


        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetail()
        {
            return await _unitOfWork.OrderDetail.GetAllAsync();
        }


        public async Task AddToOrderDetailAsync(int count, Guid productId,Guid userId)
        {
            var product = await GetProductById(productId);

            var existanceObj = await _unitOfWork.OrderDetail.GetAllAsync(x => x.UserId == userId && x.ProductId == productId);
            if(existanceObj.Count == 1 && product is not null)
            {
                var obj = existanceObj.FirstOrDefault();
                obj.TotalQuantity += count;
                obj.TotalAmount = obj.TotalAmount + (product.Price * count);

                await _unitOfWork.OrderDetail.UpdateAsync(obj);
                return;
            }

            var cartObj = new OrderDetail()
            {
                UserId = userId,
                ProductId = productId,
                TotalQuantity = count,
                TotalAmount = (product.Price * count),
            };
            await _unitOfWork.OrderDetail.CreateAsync(cartObj);
            

        }


        public async Task<OrderDetail> GetOrderDetailByIdAsync(Guid id)
        {
            return await _unitOfWork.OrderDetail.GetAsync(x=>x.Id == id);
        }

        public async Task RemoveOrderDetailAsync(OrderDetail cart)
        {
            await _unitOfWork.OrderDetail.RemoveAsync(cart);
        }

        public async Task UpdateOrderDetailAsync(Guid id, string name, string description)
        {
            
            var category = await GetOrderDetailByIdAsync(id);
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

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailByUserIdAsync(Guid userId)
        {
            return await _unitOfWork.OrderDetail.GetAllAsync(x => x.UserId == userId, includeProperties:"User,Product");
        }
    }
}
