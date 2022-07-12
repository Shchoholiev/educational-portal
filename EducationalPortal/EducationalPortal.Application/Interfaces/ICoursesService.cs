using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.Course;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;

namespace EducationalPortal.Application.Interfaces
{
    public interface ICoursesService
    {
        Task<PagedList<CourseShortDto>> GetPageAsync(PageParameters pageParameters, 
                                                     CancellationToken cancellationToken);

        Task<CourseDto> GetCourseAsync(int id, string? email, CancellationToken cancellationToken);

        Task<CourseLearnDto> GetCourseLearnAsync(int id, string email, CancellationToken cancellationToken);

        Task<CourseCreateDto> GetCourseForEditAsync(int id, CancellationToken cancellationToken);

        Task<Course> CreateAsync(CourseCreateDto courseDto, string authorEmail, 
                                 CancellationToken cancellationToken);

        Task UpdateAsync(int id, CourseCreateDto courseDto, CancellationToken cancellationToken);

        Task DeleteAsync(int id, CancellationToken cancellationToken);

        Task<int> MaterialLearnedAsync(int materialId, int courseId, string email, 
                                       CancellationToken cancellationToken);

        Task<int> MaterialUnearnedAsync(int materialId, int courseId, string email, 
                                        CancellationToken cancellationToken);
    }
}
