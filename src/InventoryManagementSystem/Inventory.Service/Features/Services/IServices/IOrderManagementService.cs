using Inventory.DataAccess.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Features.Services.IServices
{
    public interface IOrderManagementService
    {
        Task<IEnumerable<Order>> GetAllOrder();
        Task<Order> GetOrderByIdAsync(Guid id);
        Task CreateOrderAsync(Guid userId, Guid productId, int totoalQuantity, decimal unitPrice, decimal totalAmount, string orderType);
        Task UpdateOrderAsync(Guid orderId, Guid productId, int totoalQuantity, decimal unitPrice, decimal totalAmount, string orderType);
        Task RemoveOrderAsync(Order category);
        Task<IEnumerable<Order>> GetOrderByUserIdAsync(Guid userId);
        Task<IEnumerable<Product>> GetAllProductNameAsync();
        Task<IEnumerable<Order>> GetAllSaleOrder(Expression<Func<Order, bool>> filter = null);
        Task<IEnumerable<Order>> GetAllPurchaseOrder(Expression<Func<Order, bool>> filter = null);
        Task<IEnumerable<Order>> GetOrdersByDateRangeAndType(DateTime searchFrom, DateTime searchTo, string orderType);
    }
}
