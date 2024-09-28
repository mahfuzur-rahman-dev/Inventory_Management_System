using Inventory.DataAccess.Entites;
using Inventory.DataAccess.UnitOfWork;
using Inventory.Service.Features.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Service.Features.Services
{
    public class CategoryManagementService : ICategoryManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _unitOfWork.Category.GetAllAsync();
        }


        public async Task CreateCategory(string name, string description)
        {
            if (await CheckDuplicateCategoryName(name))
                throw new InvalidOperationException("A category with this name already exists.");

            var category = new Category
            { 
                Name = name, Description = description
            };
            await _unitOfWork.Category.CreateAsync(category);
        }


        private async Task<bool> CheckDuplicateCategoryName(string categoryName)
        {
            var existance = await _unitOfWork.Category.GetAllAsync(x=>x.Name == categoryName);
            if (existance.Count>0)
                return true;

            return false;
        }

        public async Task<Category> GetCategoryIdAsync(Guid id)
        {
            return await _unitOfWork.Category.GetAsync(x=>x.Id == id);
        }

        public async Task RemoveCategoryAsync(Category category)
        {
            await _unitOfWork.Category.RemoveAsync(category);
        }

        public async Task UpdateCategoryAsync(Guid id, string name, string description)
        {
            
            var category = await GetCategoryIdAsync(id);
            if (category == null)
                throw new Exception("Category not found");


            category.Description = description;
            category.Name = name;
            await _unitOfWork.Category.UpdateAsync(category);

        }

        public async Task<int> GetAllCategoryCount()
        {
            var categories  = await _unitOfWork.Category.GetAllAsync();
            return categories.Count;
        }
    }
}
