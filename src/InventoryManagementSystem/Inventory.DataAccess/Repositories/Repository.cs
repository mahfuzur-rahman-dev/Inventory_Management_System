using Inventory.DataAccess.Context;
using Inventory.DataAccess.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<T> _dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this._dbSet = db.Set<T>();
        }

        public async Task CreateAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, bool isTracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (isTracked is false)
                query = query.AsNoTracking();
            if (filter is not null)
                query = query.Where(filter);

            if (includeProperties != null)
            {
                var properties = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var property in properties)
                {
                    query = query.Include(property);
                }
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool isTracked = true, string? includeProperties = null)
        {
            IQueryable<T> query = _dbSet;
            if (isTracked is false)
                query = query.AsNoTracking();
            if (filter is not null)
                query = query.Where(filter);

            if (includeProperties != null)
            {
                foreach (var property in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);

                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            _dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
