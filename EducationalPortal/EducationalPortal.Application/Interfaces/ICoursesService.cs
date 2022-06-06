using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.Course;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces
{
    public interface ICoursesService
    {
        Task<PagedList<CourseShortDto>> GetPageAsync(PageParameters pageParameters);

        Task<CourseDto> GetCourseAsync(int id, string? email);

        Task<CourseLearnDto> GetCourseLearnAsync(int id, string email);

        Task<CourseCreateDto> GetCourseForEdit(int id);

        Task Create(CourseCreateDto courseDto, string authorEmail);

        Task Update(int id, CourseCreateDto courseDto);

        Task Delete(int id);

        Task<int> MaterialLearned(int materialId, int courseId, string email);

        Task<int> MaterialUnearned(int materialId, int courseId, string email);
    }
}
