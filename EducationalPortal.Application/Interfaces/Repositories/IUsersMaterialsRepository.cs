using EducationalPortal.Application.Models.UserStatistics;

namespace EducationalPortal.Application.Interfaces.Repositories
{
    public interface IUsersMaterialsRepository
    {
        Task<List<LearningUserStatistics>> GetLearningStatisticsForDateRangeAsync(DateTime dateStart,
            DateTime dateEnd, string userId, CancellationToken cancellationToken);

        Task<List<LearningUserStatistics>> GetLearningStatisticsDeadlinesForRangeAsync(DateTime dateStart,
           DateTime dateEnd, string userId, CancellationToken cancellationToken);

        Task<DetailedLearningUserStatistics?> GetLearningStatiscsForDayAsync(DateTime date,
            string userId, CancellationToken cancellation);
    }
}
