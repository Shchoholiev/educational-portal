using EducationalPortal.Core.Entities;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Repository
{
    public interface IUsersRepository
    {
        Task AddAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(User user);

        Task<User> GetOneAsync(int id);

        Task<IEnumerable<User>> GetAllAsync();

        Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>> predicate);
    }
}
