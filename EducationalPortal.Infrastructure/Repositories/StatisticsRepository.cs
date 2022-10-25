using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Models.QueryModels.Statistics;
using EducationalPortal.Application.Paging;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace EducationalPortal.Infrastructure.Repositories
{
    public class StatisticsRepository : IStatisticsRepository
    {
        private readonly ApplicationContext _db;

        public StatisticsRepository(ApplicationContext context)
        {
            this._db = context;
        }

        public async Task<PagedList<MaterialStatisticsQueryModel>> GetMaterialsStatisticsAsync(PageParameters pageParameters, CancellationToken cancellationToken)
        {
            var materials = await _db.Materials
                .OrderByDescending(m => m.CoursesMaterials.Count)
                .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                .Take(pageParameters.PageSize)
                .Select(m => new MaterialStatisticsQueryModel
                {
                    Name = m.Name,
                    CoursesCount = m.CoursesMaterials.Count,
                    UsersCount = m.Users.Count
                })
                .ToListAsync(cancellationToken);
            var totalCount = await this._db.Materials.CountAsync(cancellationToken);

            return new PagedList<MaterialStatisticsQueryModel>(materials, pageParameters, totalCount);
        }

        public Task<SalesStatisticsQueryModel> GetSalesStatisticsAsync(CancellationToken cancellationToken)
        {
            return _db.UsersCourses
                .Select(uc => new SalesStatisticsQueryModel
                {
                    CustomersCount = _db.Users.Count(),
                    CompletedCoursesCount = _db.Certificates.Count(),
                    SaledCoursesCount = _db.UsersCourses.Count()
                })
                .FirstAsync(cancellationToken);
        }

        public async Task<PagedList<UserStatisticsQueryModel>> GetUsersStatisticsAsync(PageParameters pageParameters, CancellationToken cancellationToken)
        {
            var users = await _db.Users
                .OrderByDescending(u => u.UsersCourses.Count)
                .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                .Take(pageParameters.PageSize)
                .Select(u => new UserStatisticsQueryModel
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Balance = u.Balance,
                    BoughtCoursesCount = u.UsersCourses.Count,
                    CompletedCoursesCount = u.Certificates.Count,
                })
                .ToListAsync(cancellationToken);
            var totalCount = await this._db.Users.CountAsync(cancellationToken);

            return new PagedList<UserStatisticsQueryModel>(users, pageParameters, totalCount);
        }
    }
}
