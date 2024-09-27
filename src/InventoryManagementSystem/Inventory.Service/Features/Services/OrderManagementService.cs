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
        private readonly IOrderDetailManagementService _orderDetailManagementService;
        public OrderManagementService(IUnitOfWork unitOfWork, IOrderDetailManagementService orderDetailManagementService)
        {
            _unitOfWork = unitOfWork;
            _orderDetailManagementService = orderDetailManagementService;
        }


        public async Task<IEnumerable<Order>> GetAllOrder()
        {
            return await _unitOfWork.Order.GetAllAsync(includeProperties:"OrderDetails");
        }


        public async Task CreateOrderAsync(List<OrderDetail> orderDetails, Guid userId, int totalQuantity, decimal totalPrice)
        {
            try
            {
                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    OrderType = OrderType.Sale.ToString(),
                    TotalAmount = totalPrice,
                    TotalQuantity = totalQuantity,
                    CustomerId = userId,
                    OrderDetails = orderDetails
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

        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetailsByUserId(Guid userId)
        {
            return await _orderDetailManagementService.GetOrderDetailByUserIdAsync(userId);
        }
    }
}
