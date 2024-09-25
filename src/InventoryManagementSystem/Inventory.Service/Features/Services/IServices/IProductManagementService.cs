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
        //Task<Product> GetProductIdAsync(Guid id);
        Task CreateProductAsync(string name, string description, decimal price, int quantity, Guid categoryId);
        //Task RemoveProductAsync(Category category);
        //Task UpdateProductAsync(Guid id, string name, string description);
        Task<IEnumerable<Category>> GetAllCategoryNameAsync();
    }
}
