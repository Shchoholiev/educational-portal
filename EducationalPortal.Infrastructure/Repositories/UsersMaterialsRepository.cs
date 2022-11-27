using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Models.UserStatistics;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Core.Enums;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace EducationalPortal.Infrastructure.Repositories
{
    public class UsersMaterialsRepository : IUsersMaterialsRepository
    {
        private readonly ApplicationContext _db;

        private readonly DbSet<UserMaterial> _table;

        public UsersMaterialsRepository(ApplicationContext context)
        {
            this._db = context;
            this._table = _db.UsersMaterials;
        }

        public Task<List<LearningUserStatistics>> GetLearningStatisticsForDateRangeAsync(DateTime dateStart, 
            DateTime dateEnd, string userId, CancellationToken cancellationToken)
        {
            return this._table.AsNoTracking()
                .Where(um => um.UserId == userId
                          && um.LearnDateUTC.Date >= dateStart.Date 
                          && um.LearnDateUTC.Date <= dateEnd.Date)
                .GroupBy(um => um.LearnDateUTC.Date)
                .Select(g => new LearningUserStatistics
                {
                    Date = DateOnly.FromDateTime(g.Key),
                    LearnedMaterialsCount = g.Count(),
                    HasDeadline = _db.SubmittedFinalTasks.Any(sft => sft.UserId == userId
                                                                  && sft.ReviewDeadlineUTC.Date == g.Key),
                })
                .ToListAsync(cancellationToken);
        }

        public Task<List<LearningUserStatistics>> GetLearningStatisticsDeadlinesForRangeAsync(DateTime dateStart,
            DateTime dateEnd, string userId, CancellationToken cancellationToken)
        {
            return this._db.SubmittedFinalTasks.AsNoTracking()
                .Where(sft => sft.UserId == userId 
                           && sft.ReviewDeadlineUTC.Date >= dateStart.Date
                           && sft.ReviewDeadlineUTC.Date <= dateEnd.Date)
                .Select(sft => new LearningUserStatistics
                {
                    Date = DateOnly.FromDateTime(sft.ReviewDeadlineUTC),
                    HasDeadline = true,
                })
                .ToListAsync(cancellationToken);
        }

        public Task<DetailedLearningUserStatistics?> GetLearningStatiscsForDayAsync(DateTime date, 
            string userId, CancellationToken cancellationToken)
        {
            return this._table.AsNoTracking()
                .Select(a => new DetailedLearningUserStatistics
                {
                    MaterialsNames = _table.Where(um => um.LearnDateUTC.Date == date.Date)
                                           .Select(um => um.Material.Name)
                                           .ToList(),
                    Deadlines = _db.SubmittedFinalTasks.Where(sft => sft.ReviewDeadlineUTC.Date == date.Date
                                                                  && sft.UserId == userId)
                                                       .Select(sft => new DeadlineUserStatistics
                                                       {
                                                           DateTimeUTC = sft.ReviewDeadlineUTC,
                                                           DeadlineType = DeadlineTypes.FinalTaskReview,
                                                       })
                                                       .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
