using Inventory.DataAccess.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Features.Services.IServices
{
    public interface ICartManagementService
    {
        Task<IEnumerable<Cart>> GetAllCart();
        Task<Cart> GetCartByIdAsync(Guid id);
        Task AddToCartAsync(int count,Guid productId,Guid userId);
        Task RemoveCartAsync(Cart category);
        Task UpdateCartAsync(Guid id, string name, string description);
        Task<Product> GetProductById(Guid productId);
        Task<IEnumerable<Cart>> GetCartByUserIdAsync(Guid userId);
    }
}
