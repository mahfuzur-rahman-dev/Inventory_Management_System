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

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _unitOfWork.Product.GetAsync(x => x.Id == id, includeProperties:"Category");
        }

        public async Task RemoveProductAsync(Product product)
        {
            await _unitOfWork.Product.RemoveAsync(product);
        }

        public async Task UpdateProductAsync(Guid id,string name, string description, decimal price, int quantity, Guid? categoryId)
        {

            var product = await GetProductByIdAsync(id);
            if (product == null)
                throw new Exception("Product not found");


            product.Description = description;
            product.Name = name;
            product.Price = price;
            product.QuantityInStock = quantity;
            if(categoryId != null)
                product.CategoryId = (Guid)categoryId;
            await _unitOfWork.Product.UpdateAsync(product);

        }
    }
}
