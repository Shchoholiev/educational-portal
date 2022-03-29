using EducationalPortal.Core.Entities;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Repository
{
    public interface ICoursesRepository
    {
        Task AddAsync(Course course);

        Task UpdateAsync(Course newCourse);

        Task DeleteAsync(Course course);

        void Attach(params object[] obj);

        Task<Course?> GetCourseAsync(int id);

        Task<Course> GetFullCourseAsync(int id);

        Task<IEnumerable<Course>> GetPageAsync(int pageSize, int pageNumber);

        Task<IEnumerable<Course>> GetPageAsync(int pageSize, int pageNumber, 
                                               Expression<Func<Course, bool>> predicate);

        Task<int> GetCountAsync();

        Task<int> GetMaterialsCountAsync(int courseId);

        Task<User> GetCourseAuthor(int courseId);
    }
}
