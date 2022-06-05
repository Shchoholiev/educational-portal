using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Common;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : EntityBase
    {
        Task AddAsync(TEntity item);

        Task UpdateAsync(TEntity item);

        Task DeleteAsync(TEntity item);

        void Attach(params object[] obj);

        Task<TEntity> GetOneAsync(int id);

        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate,
                                               params Expression<Func<TEntity, object>>[] includeProperties); 

        Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters);

        Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters, 
                                              params Expression<Func<TEntity, object>>[] includeProperties);

        Task<PagedList<TEntity>> GetPageAsync(PageParameters pageParameters,
                                              Expression<Func<TEntity, bool>> predicate,
                                              params Expression<Func<TEntity, object>>[] includeProperties);

        Task<bool> Exists(Expression<Func<TEntity, bool>> predicate);
    }
}
