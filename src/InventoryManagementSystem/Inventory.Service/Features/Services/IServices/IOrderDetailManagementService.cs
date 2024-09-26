using Inventory.DataAccess.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Features.Services.IServices
{
    public interface IOrderDetailManagementService
    {
        Task<IEnumerable<OrderDetail>> GetAllOrderDetail();
        Task<OrderDetail> GetOrderDetailByIdAsync(Guid id);
        Task AddToOrderDetailAsync(int count,Guid productId,Guid userId);
        Task RemoveOrderDetailAsync(OrderDetail category);
        Task UpdateOrderDetailAsync(Guid id, string name, string description);
        Task<Product> GetProductById(Guid productId);
        Task<IEnumerable<OrderDetail>> GetOrderDetailByUserIdAsync(Guid userId);
    }
}
