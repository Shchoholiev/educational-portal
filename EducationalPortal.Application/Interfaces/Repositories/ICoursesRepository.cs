using EducationalPortal.Application.Models.LookupModels;
using EducationalPortal.Application.Models.QueryModels;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Enums;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Interfaces.Repositories
{
    public interface ICoursesRepository
    {
        Task AddAsync(Course course, CancellationToken cancellationToken);

        Task UpdateAsync(Course newCourse, CancellationToken cancellationToken);

        Task DeleteAsync(Course course, CancellationToken cancellationToken);

        Task<Course?> GetCourseAsync(int id, CancellationToken cancellationToken);

        Task<CourseQueryModel?> GetFullCourseAsync(int id, string userId, CancellationToken cancellationToken);

        Task<PagedList<CourseShortQueryModel>> GetPageAsync(PageParameters pageParameters, CancellationToken cancellationToken);

        Task<PagedList<CourseShortQueryModel>> GetPageAsync(PageParameters pageParameters, string filter, 
            CoursesOrderBy orderBy, bool isAscending, CancellationToken cancellationToken);

        Task<List<CourseLookupModel>> GetLookupModelsAsync(IEnumerable<int> skillIds, IEnumerable<int> courseIds, 
            string userId, CancellationToken cancellationToken);
        
        Task<List<CourseLookupModel>> GetLookupModelsAsync(IEnumerable<int> skillIds, IEnumerable<int> courseIds,
            IEnumerable<int> materialIds, string userId, CancellationToken cancellationToken);

        Task<List<CourseShortQueryModel>> GetCoursesAsync(IEnumerable<int> courseIds, CancellationToken cancellationToken);

        Task<int> GetMaterialsCountAsync(int courseId, CancellationToken cancellationToken);

        Task<User?> GetCourseAuthor(int courseId, CancellationToken cancellationToken);

        Task<List<Skill>> GetSkillsAsync(IEnumerable<int> skillIds, CancellationToken cancellationToken);
    }
}
