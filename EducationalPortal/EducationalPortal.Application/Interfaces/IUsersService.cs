using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Interfaces
{
    public interface IUsersService
    {
        Task UpdateUserAsync(User user);

        Task DeleteAsync(string id);

        Task<User?> GetUserAsync(string email);

        Task<User?> GetUserWithMaterialsAsync(string email);

        Task<User?> GetAuthorAsync(string email);

        Task AddUsersCoursesAsync(UsersCourses usersCourses);

        Task<UsersCourses?> GetUsersCoursesAsync(int courseId, string email);

        Task<PagedList<UsersCourses>> GetUsersCoursesPageAsync(string email, PageParameters pageParameters,
                                                               Expression<Func<UsersCourses, bool>> predicate);

        Task UpdateUsersCoursesAsync(UsersCourses usersCourses);

        Task AddAcquiredSkills(int courseId, string email);

        Task<int> GetLearnedMaterialsCountAsync(int courseId, string email);

        Task SaveDbAsync();
    }
}
