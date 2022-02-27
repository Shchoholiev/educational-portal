using EducationalPortal.Core.Entities;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Repository
{
    public interface IUserRepository
    {
        Task Add(User user);

        Task Update(User user);

        Task Delete(User user);

        Task<User> GetOne(int id);

        Task<IEnumerable<User>> GetAll();

        Task<IEnumerable<User>> GetAll(Expression<Func<User, bool>> predicate);
    }
}
