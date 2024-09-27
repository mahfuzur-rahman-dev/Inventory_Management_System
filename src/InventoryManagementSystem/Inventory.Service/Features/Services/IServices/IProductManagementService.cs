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
        Task CreateProductAsync(string name, string description, decimal buyingPrice, decimal sellingPrice, int quantity, Guid categoryId);
        Task RemoveProductAsync(Product category);
        Task UpdateProductAsync(Guid id, string name, string description, decimal buyingPrice, decimal sellingPrice, int quantity, Guid? categoryId);
        Task<IEnumerable<Category>> GetAllCategoryNameAsync();
    }
}
