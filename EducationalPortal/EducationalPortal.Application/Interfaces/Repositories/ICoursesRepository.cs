using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Interfaces.Repositories
{
    public interface ICoursesRepository
    {
        Task AddAsync(Course course, CancellationToken cancellationToken);

        Task UpdateAsync(Course newCourse, CancellationToken cancellationToken);

        Task DeleteAsync(Course course, CancellationToken cancellationToken);

        void Detach(object entity, CancellationToken cancellationToken);

        Task<Course?> GetCourseAsync(int id, CancellationToken cancellationToken);

        Task<Course> GetFullCourseAsync(int id, CancellationToken cancellationToken);

        Task<PagedList<Course>> GetPageAsync(PageParameters pageParameters, CancellationToken cancellationToken);

        Task<PagedList<Course>> GetPageAsync(PageParameters pageParameters, 
                                             Expression<Func<Course, bool>> predicate, 
                                             CancellationToken cancellationToken);

        Task<int> GetMaterialsCountAsync(int courseId, CancellationToken cancellationToken);

        Task<User> GetCourseAuthor(int courseId, CancellationToken cancellationToken);

        Task<bool> Exists(Expression<Func<Course, bool>> predicate, CancellationToken cancellationToken);
    }
}
