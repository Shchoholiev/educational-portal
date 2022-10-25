using EducationalPortal.Application.Models.StatisticsModel;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces
{
    public interface IStatisticsService
    {
        Task<PagedList<MaterialStatisticsModel>> GetMaterialsStatisticsAsync(PageParameters pageParameters,
            CancellationToken cancellationToken);

        Task<SalesStatisticsModel> GetSalesStatisticsAsync(PageParameters pageParameters, CancellationToken cancellationToken);

        Task<UsersStatisticsModel> GetUsersStatisticsAsync(PageParameters pageParameters, CancellationToken cancellationToken);
    
        Task<PagedList<CourseStatisticsModel>> GetUCoursesStatisticsAsync(PageParameters pageParameters, CancellationToken cancellationToken);
    }
}
