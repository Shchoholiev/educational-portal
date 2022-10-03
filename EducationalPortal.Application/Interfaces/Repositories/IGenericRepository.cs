using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Common;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : EntityBase
    {
        Task AddAsync(TEntity item, CancellationToken cancellationToken);

        Task UpdateAsync(TEntity item, CancellationToken cancellationToken);

        Task DeleteAsync(TEntity item, CancellationToken cancellationToken);

        void Attach(params object[] obj);

        Task<TEntity?> GetOneAsync(int id, CancellationToken cancellationToken);

        Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);

        Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate,
                                               CancellationToken cancellationToken,
                                               params Expression<Func<TEntity, object>>[] includeProperties); 

        Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters, CancellationToken cancellationToken);

        Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters,
                                              CancellationToken cancellationToken,
                                              params Expression<Func<TEntity, object>>[] includeProperties);

        Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters,
                                              Expression<Func<TEntity, bool>> predicate,
                                              CancellationToken cancellationToken,
                                              params Expression<Func<TEntity, object>>[] includeProperties);

        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
    }
}
