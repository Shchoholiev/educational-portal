using System.Linq.Expressions;
using EducationalPortal.Application.Paging;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using EducationalPortal.Core.Common;
using EducationalPortal.Application.Interfaces.Repositories;

namespace EducationalPortal.Infrastructure.Repositories
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

        public async Task AddAsync(TEntity item, CancellationToken cancellationToken)
        {
            await this._table.AddAsync(item, cancellationToken);
            await this.SaveAsync(cancellationToken);
        }

        public async Task UpdateAsync(TEntity item, CancellationToken cancellationToken)
        {
            this._table.Update(item);
            await this.SaveAsync(cancellationToken);
        }

        public async Task DeleteAsync(TEntity item, CancellationToken cancellationToken)
        {
            this._table.Remove(item);
            await this.SaveAsync(cancellationToken);
        }

        public Task<TEntity?> GetOneAsync(int id, CancellationToken cancellationToken)
        {
            return this._table.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }

        public Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> predicate, 
                                          CancellationToken cancellationToken)
        {
            return this._table.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate,
            CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this._table.AsNoTracking().Where(predicate);
            return this.Include(query, includeProperties).ToListAsync(cancellationToken);
        }

        public async Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters, 
                                                           CancellationToken cancellationToken)
        {
            var entities = await this._table
                                     .AsNoTracking()
                                     .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                     .Take(pageParameters.PageSize)
                                     .ToListAsync(cancellationToken);
            var totalCount = await this._table.CountAsync(cancellationToken);

            return new PagedList<TEntity>(entities, pageParameters, totalCount);
        }

        public async Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters,
                                                 CancellationToken cancellationToken,
                                                 params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this._table.AsNoTracking()
                                   .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                   .Take(pageParameters.PageSize);
            var entities = await this.Include(query, includeProperties).ToListAsync(cancellationToken);
            var totalCount = await this._table.CountAsync(cancellationToken);

            return new PagedList<TEntity>(entities, pageParameters, totalCount);
        }

        public async Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters,
                                                 Expression<Func<TEntity, bool>> predicate,
                                                 CancellationToken cancellationToken,
                                                 params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = this._table.AsNoTracking()
                                   .Where(predicate)
                                   .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                   .Take(pageParameters.PageSize);
            var entities = await this.Include(query, includeProperties).ToListAsync(cancellationToken);
            var totalCount = await this._table.CountAsync(predicate, cancellationToken);

            return new PagedList<TEntity>(entities, pageParameters, totalCount);
        }

        public void Attach(params object[] obj)
        {
            this._db.AttachRange(obj);
        }

        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, 
                                            CancellationToken cancellationToken)
        {
            return this._table.AnyAsync(predicate, cancellationToken);
        }

        private IQueryable<TEntity> Include(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }

        private async Task SaveAsync(CancellationToken cancellationToken)
        {
            await this._db.SaveChangesAsync(cancellationToken);
        }
    }
}