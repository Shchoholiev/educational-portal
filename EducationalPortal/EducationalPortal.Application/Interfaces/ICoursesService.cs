using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.Course;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;

namespace EducationalPortal.Application.Interfaces
{
    public interface ICoursesService
    {
        Task<PagedList<CourseShortDto>> GetPageAsync(PageParameters pageParameters);

        Task<CourseDto> GetCourseAsync(int id, string? email);

        Task<CourseLearnDto> GetCourseLearnAsync(int id, string email);

        Task<CourseCreateDto> GetCourseForEditAsync(int id);

        Task<Course> CreateAsync(CourseCreateDto courseDto, string authorEmail);

        Task UpdateAsync(int id, CourseCreateDto courseDto);

        Task DeleteAsync(int id);

        Task<int> MaterialLearnedAsync(int materialId, int courseId, string email);

        Task<int> MaterialUnearnedAsync(int materialId, int courseId, string email);
    }
}
