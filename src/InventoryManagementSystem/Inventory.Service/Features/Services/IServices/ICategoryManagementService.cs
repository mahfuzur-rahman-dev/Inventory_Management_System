using Inventory.DataAccess.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Features.Services.IServices
{
    public interface ICategoryManagementService
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategoryIdAsync(Guid id);
        Task CreateCategory(string name, string description);
        Task RemoveCategoryAsync(Category category);
        Task UpdateCategoryAsync(Guid id, string name, string description);
        Task<int> GetAllCategoryCount();
    }
}
