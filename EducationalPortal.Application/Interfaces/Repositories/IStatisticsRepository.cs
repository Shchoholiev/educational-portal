using EducationalPortal.Application.Models.QueryModels.Statistics;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces.Repositories
{
    public interface IStatisticsRepository
    {
        Task<PagedList<MaterialStatisticsQueryModel>> GetMaterialsStatisticsAsync(PageParameters pageParameters, 
            CancellationToken cancellationToken);

        Task<SalesStatisticsQueryModel> GetSalesStatisticsAsync(CancellationToken cancellationToken);

        Task<PagedList<UserStatisticsQueryModel>> GetUsersStatisticsAsync(PageParameters pageParameters,
            CancellationToken cancellationToken);
    }
}
