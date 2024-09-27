using Inventory.DataAccess.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Features.Services.IServices
{
    public interface IOrderManagementService
    {
        Task<IEnumerable<Order>> GetAllOrder();
        Task<Order> GetOrderByIdAsync(Guid id);
        Task CreateOrderAsync(List<OrderDetail> orderDetails,Guid userId, int totalQuantity, decimal totalPrice);
        Task RemoveOrderAsync(Order category);
        Task<IEnumerable<Order>> GetOrderByUserIdAsync(Guid userId);
        Task<IEnumerable<OrderDetail>> GetAllOrderDetailsByUserId(Guid userId);
    }
}
