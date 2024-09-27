using Inventory.DataAccess.Entites;
using Inventory.DataAccess.Enums;
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
    public class OrderManagementService : IOrderManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductManagementService _productManagementService;
        public OrderManagementService(IUnitOfWork unitOfWork,  IProductManagementService productManagementService)
        {
            _unitOfWork = unitOfWork;
            _productManagementService = productManagementService;
        }


        public async Task<IEnumerable<Order>> GetAllOrder()
        {
            return await _unitOfWork.Order.GetAllAsync(includeProperties:"OrderDetails");
        }


        public async Task CreateOrderAsync(Guid userId, Guid productId, int totoalQuantity, decimal unitPrice, decimal totalAmount, string orderType)
        {
            
            try
            {
                var product = await _productManagementService.GetProductByIdAsync(productId);
                if (product == null)
                    throw new Exception("Product not found");

                var order = new Order
                {
                   ProductId = productId,
                   TotalQuantity = totoalQuantity,
                   TotalAmount = totalAmount,
                   UnitPrice = unitPrice,
                   OrderType = orderType,
                   CreatedDate = DateTime.Now,
                   Product = product
                };

                await _unitOfWork.Order.CreateAsync(order);

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

        }


        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            return await _unitOfWork.Order.GetAsync(x=>x.Id == id);
        }

        public async Task RemoveOrderAsync(Order order)
        {
            await _unitOfWork.Order.RemoveAsync(order);
        }


        public async Task<IEnumerable<Order>> GetOrderByUserIdAsync(Guid userId)
        {
            return await _unitOfWork.Order.GetAllAsync(x => x.CustomerId == userId, includeProperties: "User,Product");
        }

        

        public async Task<IEnumerable<Product>> GetAllProductNameAsync()
        {
            return await _productManagementService.GetAllProducts();
        }

    }
}
