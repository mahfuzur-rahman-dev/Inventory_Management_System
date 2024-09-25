using Inventory.DataAccess.Context;
using Inventory.DataAccess.Repositories;
using Inventory.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IProductRepository Product {  get; set; }
        public ICategoryRepository Category {  get; set; }
        public ICartRepository Cart {  get; set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Product = new ProductRepository(_context);
            Category = new CategoryRepository(_context);
            Cart = new CartRepository(_context);
        }


    }
}
