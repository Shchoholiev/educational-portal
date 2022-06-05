using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.Course;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces
{
    public interface ICoursesService
    {
        Task<PagedList<CourseShortDto>> GetPageAsync(PageParameters pageParameters);

        Task<CourseDto> GetCourseAsync(int id);

        Task<CourseLearnDto> GetCourseLearnAsync(int id);

        Task<CourseCreateDto> GetCourseForEdit(int id);

        Task Create(CourseCreateDto courseDto);

        Task Update(CourseCreateDto courseDto);

        Task Delete(int id);

        Task<int> MaterialLearned(int materialId, int courseId);

        Task<int> MaterialUnearned(int materialId, int courseId);
    }
}
