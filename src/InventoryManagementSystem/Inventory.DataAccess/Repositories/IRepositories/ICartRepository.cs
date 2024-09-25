using Inventory.DataAccess.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataAccess.Repositories.IRepositories
{
    public interface ICartRepository  : IRepository<Cart>
    {
        Task UpdateAsync(Cart entity);
    }
}
