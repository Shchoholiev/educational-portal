using System.Linq.Expressions;

namespace EducationalPortal.Core.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task Add(TEntity item);

        Task Update(TEntity item);

        Task Delete(TEntity item);

        Task Delete(int id);

        Task Save();

        Task Attach(params object[] obj);

        Task<TEntity> GetOne(int? id);

        Task<TEntity> GetOne(int? id, params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetAll();

        Task<IEnumerable<TEntity>> GetAll(params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate,
                                   params Expression<Func<TEntity, object>>[] includeProperties);

        Task<IEnumerable<TEntity>> GetPage(int pageSize, int pageNumber);
    }
}
