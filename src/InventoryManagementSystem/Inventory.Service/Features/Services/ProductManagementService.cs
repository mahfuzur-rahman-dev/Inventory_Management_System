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
    public class ProductManagementService : IProductManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryManagementService _categoryManagementService;
        public ProductManagementService(IUnitOfWork unitOfWork , ICategoryManagementService categoryManagementService)
        {
            _unitOfWork = unitOfWork;
            _categoryManagementService = categoryManagementService; 
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _unitOfWork.Product.GetAllAsync(includeProperties: "Category");
        }


        public async Task CreateProductAsync(string name, string description, decimal price, int quantity, Guid categoryId)
        {
            if (await CheckDuplicateName(name))
                throw new InvalidOperationException("A product with this name already exists.");

            var product = new Product
            { 
                Name = name,
                Description = description,
                Price = price,
                QuantityInStock = quantity,
                CategoryId = categoryId
            };
            await _unitOfWork.Product.CreateAsync(product);
        }


        private async Task<bool> CheckDuplicateName(string name)
        {
            var existance = await _unitOfWork.Product.GetAllAsync(x => x.Name == name);
            if (existance.Count > 0)
                return true;

            return false;
        }

        public async Task<IEnumerable<Category>> GetAllCategoryNameAsync()
        {
            return await _categoryManagementService.GetAllCategories();
        }

        //public async Task<Category> GetCategoryIdAsync(Guid id)
        //{
        //    return await _unitOfWork.Category.GetAsync(x=>x.Id == id);
        //}

        //public async Task RemoveCategoryAsync(Category category)
        //{
        //    await _unitOfWork.Category.RemoveAsync(category);
        //}

        //public async Task UpdateCategoryAsync(Guid id, string name, string description)
        //{

        //    var category = await GetCategoryIdAsync(id);
        //    if (category == null)
        //        throw new Exception("Category not found");


        //    category.Description = description;
        //    category.Name = name;
        //    await _unitOfWork.Category.UpdateAsync(category);

        //}
    }
}
