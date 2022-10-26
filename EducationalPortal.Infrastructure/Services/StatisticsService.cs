using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Mapping;
using EducationalPortal.Application.Models.StatisticsModel;
using EducationalPortal.Application.Paging;
using Microsoft.Extensions.Logging;

namespace EducationalPortal.Infrastructure.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IStatisticsRepository _statisticsRepository;
        
        private readonly ICoursesRepository _coursesRepository;

        private readonly ILogger _logger;

        private readonly Mapper _mapper = new();

        public StatisticsService(IStatisticsRepository statisticsRepository, ICoursesRepository coursesRepository,
                                ILogger<StatisticsService> logger)
        {
            _statisticsRepository = statisticsRepository;
            _coursesRepository = coursesRepository;
            _logger = logger;
        }

        public async Task<PagedList<MaterialStatisticsModel>> GetMaterialsStatisticsAsync(PageParameters pageParameters, 
            CancellationToken cancellationToken)
        {
            var materials = await _statisticsRepository.GetMaterialsStatisticsAsync(pageParameters, cancellationToken);
            var dtos = _mapper.Map(materials);

            this._logger.LogInformation($"Returned materials statistics page from database.");

            return dtos;
        }

        public async Task<SalesStatisticsModel> GetSalesStatisticsAsync(CancellationToken cancellationToken)
        {
            var statistic = await _statisticsRepository.GetSalesStatisticsAsync(cancellationToken);
            var dto = _mapper.Map(statistic);
            dto.CompletedCoursesPercentage = dto.CompletedCoursesCount * 100 / dto.SaledCoursesCount;

            this._logger.LogInformation($"Returned Sales statistics from database.");

            return dto;
        }

        public async Task<PagedList<CourseStatisticsModel>> GetCoursesStatisticsAsync(PageParameters pageParameters, CancellationToken cancellationToken)
        {
            var courses = await _statisticsRepository.GetCoursesStatisticsAsync(pageParameters, cancellationToken);
            var dtos = _mapper.Map(courses);

            this._logger.LogInformation($"Returned courses statistics page from database.");

            return dtos;
        }

        public async Task<UsersStatisticsModel> GetUsersStatisticsAsync(PageParameters pageParameters, CancellationToken cancellationToken)
        {
            var users = await _statisticsRepository.GetUsersStatisticsAsync(pageParameters, cancellationToken);
            var dtos = _mapper.Map(users);
            var statistics = new UsersStatisticsModel
            {
                Users = dtos,
                AverageCompletedCoursesPercentage = dtos.Sum(u => u.CompletedCoursesPercentage) / dtos.Where(u => u.BoughtCoursesCount > 0).Count(),
            };

            this._logger.LogInformation($"Returned Sales statistics from database.");

            return statistics;
        }
    }
}
