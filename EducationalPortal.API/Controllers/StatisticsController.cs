using EducationalPortal.Application.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Models.StatisticsModel;

namespace EducationalPortal.API.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatisticsController : ApiControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            this._statisticsService = statisticsService;
        }

        [HttpGet("materials/{PageSize}/{PageNumber}")]
        public async Task<IEnumerable<MaterialStatisticsModel>> GetMaterialsStatisticsAsync(
            [FromRoute] PageParameters pageParameters, CancellationToken cancellationToken)
        {
            var statistics = await this._statisticsService.GetMaterialsStatisticsAsync(pageParameters, cancellationToken);
            this.SetPagingMetadata(statistics);
            return statistics;
        }

        [HttpGet("sales")]
        public async Task<SalesStatisticsModel> GetSalesStatisticsAsync(CancellationToken cancellationToken)
        {
            return await this._statisticsService.GetSalesStatisticsAsync(cancellationToken);
        }

        [HttpGet("courses/{PageSize}/{PageNumber}")]
        public async Task<IEnumerable<CourseStatisticsModel>> GetCoursesStatisticsAsync(
            [FromRoute] PageParameters pageParameters, CancellationToken cancellationToken)
        {
            var statistics = await this._statisticsService.GetCoursesStatisticsAsync(pageParameters, cancellationToken);
            this.SetPagingMetadata(statistics);
            return statistics;
        }

        [HttpGet("users/{PageSize}/{PageNumber}")]
        public async Task<UsersStatisticsModel> GetUsersStatisticsAsync(
            [FromRoute] PageParameters pageParameters, CancellationToken cancellationToken)
        {
            var statistics = await this._statisticsService.GetUsersStatisticsAsync(pageParameters, cancellationToken);
            this.SetPagingMetadata(statistics.Users);
            return statistics;
        }
    }
}
