using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities.JoinEntities;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Interfaces.Repositories
{
    public interface IUsersCoursesRepository
    {
        Task<UsersCourses?> GetUsersCoursesAsync(int courseId, string email);

        Task<PagedList<UsersCourses>> GetUsersCoursesPageAsync(string email, PageParameters pageParameters,
                                                               Expression<Func<UsersCourses, bool>> predicate);

        Task AddUsersCoursesAsync(UsersCourses usersCourses);

        Task UpdateUsersCoursesAsync(UsersCourses usersCourses);

        Task<int> GetLearnedMaterialsCountAsync(int courseId, string email);

        Task<bool> ExistsAsync(int courseId, string email);
    }
}
