using Inventory.DataAccess.Context;
using Inventory.DataAccess.Entites;
using Inventory.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataAccess.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(Cart entity)
        {
            _context.Carts.Update(entity);
            await SaveAsync();
        }

    }
}
