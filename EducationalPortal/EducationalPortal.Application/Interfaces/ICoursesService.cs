using EducationalPortal.Core.Entities;

namespace EducationalPortal.Application.Interfaces
{
    public interface ICoursesService
    {
        Task AddCourse(Course course);

        Task<Course> GetCourse(int id);

        Task<IEnumerable<Course>> GetCourses();

        Task<IEnumerable<Course>> GetPage(int pageSize, int pageNumber);

        Task UpdateCourse(Course course);

        Task DeleteCourse(int id);

        Task<IEnumerable<Skill>> GetSkills();
    }
}
