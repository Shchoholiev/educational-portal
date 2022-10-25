using EducationalPortal.Application.Models.StatisticsModel;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces
{
    public interface IStatisticsService
    {
        Task<PagedList<MaterialStatisticsModel>> GetMaterialsStatisticsAsync(PageParameters pageParameters,
            CancellationToken cancellationToken);

        Task<SalesStatisticsModel> GetSalesStatisticsAsync(CancellationToken cancellationToken);

        Task<UsersStatisticsModel> GetUsersStatisticsAsync(PageParameters pageParameters, CancellationToken cancellationToken);
    
        Task<PagedList<CourseStatisticsModel>> GetCoursesStatisticsAsync(PageParameters pageParameters, CancellationToken cancellationToken);
    }
}
