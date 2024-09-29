using Inventory.DataAccess.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Features.Services.IServices
{
    public interface IProductManagementService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductByIdAsync(Guid id);
        Task CreateProductAsync(string name, string description, Guid categoryId,Guid userId);
        Task RemoveProductAsync(Product category);
        Task UpdateProductAsync(Guid id, string name, string description, Guid categoryId);
        Task<IEnumerable<Category>> GetAllCategoryNameAsync();
        Task<int> GetAllStockProductCount();
        Task<int> ProductStockCount();
    }
}
