using EducationalPortal.Application.Paging;
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

        Task<User?> GetUserWithMaterialsAsync(string email);

        Task<User?> GetAuthorAsync(string email);

        Task<UsersCourses?> GetUsersCoursesAsync(int courseId, string email);

        Task<PagedList<UsersCourses>> GetUsersCoursesPageAsync(string email, PageParameters pageParameters,
                                                            Expression<Func<UsersCourses, bool>> predicate);

        Task AddUsersCoursesAsync(UsersCourses usersCourses);

        Task UpdateUsersCoursesAsync(UsersCourses usersCourses);

        Task<int> GetLearnedMaterialsCountAsync(int courseId, string email);
    }
}
