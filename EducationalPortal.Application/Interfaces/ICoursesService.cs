using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.Course;
using EducationalPortal.Application.Models.LookupModels;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Enums;

namespace EducationalPortal.Application.Interfaces
{
    public interface ICoursesService
    {
        Task<PagedList<CourseShortDto>> GetPageAsync(PageParameters pageParameters, string userId,
            CancellationToken cancellationToken);

        Task<PagedList<CourseShortDto>> GetFilteredPageAsync(PageParameters pageParameters,
            string filter, CoursesOrderBy orderBy, bool isAscending, string userId, CancellationToken cancellationToken);

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

        Task<List<CourseShortDto>> GetCoursesByAutomatedSearchAsync(List<SkillLookupModel> skillLookups, 
            string userId, CancellationToken cancellationToken);
        
        Task<List<CourseShortDto>> GetCoursesByAutomatedSearchBasedOnTimeAsync(List<SkillLookupModel> skillLookups, 
            string userId, CancellationToken cancellationToken);

        Task<byte[]> GetPdfForAutomatedSearchAsync(List<SkillLookupModel> skillLookups,
            string userId, CancellationToken cancellationToken);

        Task<byte[]> GetPdfForAutomatedSearchBasedOnTimeAsync(List<SkillLookupModel> skillLookups,
            string userId, CancellationToken cancellationToken);
    }
}
