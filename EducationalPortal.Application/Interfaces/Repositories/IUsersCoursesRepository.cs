using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities.JoinEntities;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Interfaces.Repositories
{
    public interface IUsersCoursesRepository
    {
        Task<UsersCourses?> GetUsersCoursesAsync(int courseId, string email, CancellationToken cancellationToken);

        Task<PagedList<UsersCourses>> GetUsersCoursesPageAsync(string email, PageParameters pageParameters,
                                                               Expression<Func<UsersCourses, bool>> predicate, 
                                                               CancellationToken cancellationToken);

        Task AddUsersCoursesAsync(UsersCourses usersCourses, CancellationToken cancellationToken);

        Task UpdateUsersCoursesAsync(UsersCourses usersCourses, CancellationToken cancellationToken);

        Task<int> GetLearnedMaterialsCountAsync(int courseId, string email, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(int courseId, string email, CancellationToken cancellationToken);
    }
}
