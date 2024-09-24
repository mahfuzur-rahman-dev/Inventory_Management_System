using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataAccess.Repositories.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task SaveAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, bool isTracked = true, string? includeProperties = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool isTracked = true, string? includeProperties = null);
    }
}
