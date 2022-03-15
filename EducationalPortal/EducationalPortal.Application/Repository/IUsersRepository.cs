using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Repository
{
    public interface IUsersRepository
    {
        Task AddAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(User user);

        Task<User?> GetUserAsync(string email);

        Task<User?> GetUserWithSkillsAsync(string email);

        Task<IEnumerable<User>> GetAllAsync(); // ?

        Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>> predicate); // ?

        Task<IEnumerable<UsersCourses>> GetUsersCoursesPageAsync(string email, int pageSize, int pageNumber);

        Task<int> GetUsersCoursesCountAsync(string email);
    }
}
