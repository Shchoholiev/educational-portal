using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Repository
{
    public interface ICoursesRepository
    {
        Task AddAsync(Course course);

        Task UpdateAsync(Course newCourse);

        Task DeleteAsync(Course course);

        Task<Course?> GetCourseAsync(int id);

        Task<Course> GetFullCourseAsync(int id);

        Task<PagedList<Course>> GetPageAsync(PageParameters pageParameters);

        Task<PagedList<Course>> GetPageAsync(PageParameters pageParameters, 
                                             Expression<Func<Course, bool>> predicate);

        Task<int> GetMaterialsCountAsync(int courseId);

        Task<User> GetCourseAuthor(int courseId);

        Task<bool> Exists(Expression<Func<Course, bool>> predicate);
    }
}
