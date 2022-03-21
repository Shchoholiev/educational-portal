using System.Linq.Expressions;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace EducationalPortal.Infrastructure.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : EntityBase
    {
        private readonly ApplicationContext _db;
        private readonly DbSet<TEntity> _table;

        public GenericRepository()
        {
            this._db = new ApplicationContext();
            this._table = _db.Set<TEntity>();
        }

        public async Task AddAsync(TEntity item)
        {
            await this._table.AddAsync(item);
            await this.SaveAsync();
        }

        public async Task UpdateAsync(TEntity item)
        {
            this._table.Update(item);
            await this.SaveAsync();
        }

        public async Task DeleteAsync(TEntity item)
        {
            this._table.Remove(item);
            await this.SaveAsync();
        }

        public async Task<TEntity> GetOneAsync(int id)
        {
            return await this._table.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<TEntity> GetOneAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var items = await this.GetAllAsync(includeProperties);
            return items.FirstOrDefault(i => i.Id == id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await this._table.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = this._table.AsNoTracking();
            return await this.Include(query, includeProperties).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate,
                                                     params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = this._table.AsNoTracking().Where(predicate);
            return await this.Include(query, includeProperties).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetPageAsync(int pageSize, int pageNumber)
        {
            IQueryable<TEntity> query = this._table.AsNoTracking()
                                                 .Skip((pageNumber - 1) * pageSize)
                                                 .Take(pageSize);
            return await this.Include(query).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetPageAsync(int pageSize, int pageNumber,
                                                       params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = this._table.AsNoTracking()
                                                 .Skip((pageNumber - 1) * pageSize)
                                                 .Take(pageSize);
            return await this.Include(query, includeProperties).ToListAsync();
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await this._table.Where(predicate).CountAsync();
        }

        public void Attach(params object[] obj)
        {
            foreach (var o in obj)
            {
                this._db.Attach(o);
            }
        }

        private IQueryable<TEntity> Include(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties
                .Aggregate(query, (current, includeProperty)
                    => current.Include(includeProperty));
        }

        private async Task SaveAsync()
        {
            await this._db.SaveChangesAsync();
        }
    }
}