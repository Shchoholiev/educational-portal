using System.Linq.Expressions;
using EducationalPortal.Application.Paging;
using EducationalPortal.Application.IRepositories;
using EducationalPortal.Core.Entities;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace EducationalPortal.Infrastructure.IRepositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : EntityBase
    {
        private readonly ApplicationContext _db;
        private readonly DbSet<TEntity> _table;

        public GenericRepository(ApplicationContext context)
        {
            this._db = context;
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


        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate,
                                                     params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = this._table.AsNoTracking().Where(predicate);
            return await this.Include(query, includeProperties).ToListAsync();
        }

        public async Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters)
        {
            var entities = await this._table
                                     .AsNoTracking()
                                     .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                     .Take(pageParameters.PageSize)
                                     .ToListAsync();
            var totalCount = await this._table.CountAsync();

            return new PagedList<TEntity>(entities, pageParameters, totalCount);
        }

        public async Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters,
                                                    params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = this._table
                                            .AsNoTracking()
                                            .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                            .Take(pageParameters.PageSize);
            var entities = await this.Include(query, includeProperties).ToListAsync();
            var totalCount = await this._table.CountAsync();

            return new PagedList<TEntity>(entities, pageParameters, totalCount);
        }

        public async Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters,
                                                 Expression<Func<TEntity, bool>> predicate,
                                                 params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = this._table
                                            .AsNoTracking()
                                            .Where(predicate)
                                            .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                            .Take(pageParameters.PageSize);
            var entities = await this.Include(query, includeProperties).ToListAsync();
            var totalCount = await this._table.Where(predicate).CountAsync();

            return new PagedList<TEntity>(entities, pageParameters, totalCount);
        }

        public void Attach(params object[] obj)
        {
            foreach (var o in obj)
            {
                this._db.Attach(o);
            }
        }

        public async Task<bool> Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return await this._table.AnyAsync(predicate);
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