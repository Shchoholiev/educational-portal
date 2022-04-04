using EducationalPortal.Application.Paging;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity item);

        Task UpdateAsync(TEntity item);

        Task DeleteAsync(TEntity item);

        void Attach(params object[] obj);

        Task<TEntity> GetOneAsync(int id);

        Task<TEntity> GetOneAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate,
                                   params Expression<Func<TEntity, object>>[] includeProperties); 

        Task<IEnumerable<TEntity>> GetPageAsync(PageParameters pageParameters);

        Task<IEnumerable<TEntity>> GetPageAsync(PageParameters pageParameters, 
                                   params Expression<Func<TEntity, object>>[] includeProperties);

        Task<int> GetCountAsync(Expression<Func<TEntity, bool>> predicate);

        Task<bool> Exists(Expression<Func<TEntity, bool>> predicate);
    }
}
