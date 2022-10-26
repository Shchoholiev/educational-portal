using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Models.QueryModels.Statistics;
using EducationalPortal.Application.Paging;
using EducationalPortal.Infrastructure.EF;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Text;

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
            var json = new StringBuilder();
            using (var command = this._db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = 
                    "SELECT " +
                        "m.[Name], " +
                        "(SELECT COUNT(*) " +
                        "FROM dbo.CoursesMaterials AS cm " +
                        "WHERE cm.MaterialId = m.Id" +
                        ") as CoursesCount, " +
                        "(SELECT COUNT(*) " +
                        "FROM dbo.MaterialsBaseUser AS mu " +
                        "WHERE mu.MaterialsId = m.Id" +
                        ") as UsersCount " +
                    "FROM dbo.Materials AS m " +
                    "GROUP BY m.Id, m.[Name] " +
                    "ORDER BY CoursesCount DESC, UsersCount DESC " +
                    "OFFSET @offset ROWS " +
                    "FETCH NEXT @pageSize ROWS ONLY " +
                    "FOR JSON PATH";

                command.Parameters.Add(new SqlParameter("@offset", (pageParameters.PageNumber - 1) * pageParameters.PageSize));
                command.Parameters.Add(new SqlParameter("@pageSize", pageParameters.PageSize));

                await this._db.Database.OpenConnectionAsync(cancellationToken);
                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        json.Append(reader.GetValue(0).ToString());
                    }
                }
            }
            var materials = JsonConvert.DeserializeObject<List<MaterialStatisticsQueryModel>>(json.ToString());
            var totalCount = await this._db.Materials.CountAsync(cancellationToken);

            return new PagedList<MaterialStatisticsQueryModel>(materials, pageParameters, totalCount);
        }

        public async Task<SalesStatisticsQueryModel> GetSalesStatisticsAsync(CancellationToken cancellationToken)
        {
            var json = new StringBuilder();
            using (var command = this._db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText =
                    "SELECT " +
                        "(SELECT COUNT(*) " +
                        "FROM dbo.Users " +
                        ") AS CustomersCount, " +
                        "(SELECT COUNT(*) " +
                        "FROM dbo.[Certificates] " +
                        ") AS CompletedCoursesCount, " +
                        "(SELECT COUNT(*) " +
                        "FROM dbo.UsersCourses " +
                        ") AS SaledCoursesCount " +
                    "FOR JSON PATH";

                await this._db.Database.OpenConnectionAsync(cancellationToken);
                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        json.Append(reader.GetValue(0).ToString());
                    }
                }
            }
            var statistics = JsonConvert.DeserializeObject<List<SalesStatisticsQueryModel>>(json.ToString());

            return statistics.First();
        }

        public async Task<PagedList<CourseStatisticsQueryModel>> GetCoursesStatisticsAsync(PageParameters pageParameters, CancellationToken cancellationToken)
        {
            var json = new StringBuilder();
            using (var command = this._db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText =
                    "SELECT " +
                        "c.Id, " +
                        "c.[Name], " +
                        "c.UpdateDateUTC, " +
                        "c.Price, " +
                        "COUNT(uc.UserId) AS StudentsCount " +
                    "FROM dbo.Courses AS c " +
                    "LEFT JOIN dbo.UsersCourses AS uc ON uc.CourseId = c.Id " +
                    "GROUP BY c.Id, c.[Name], c.UpdateDateUTC, c.Price " +
                    "ORDER BY StudentsCount DESC, c.UpdateDateUTC DESC " +
                    "OFFSET @offset ROWS " +
                    "FETCH NEXT @pageSize ROWS ONLY " +
                    "FOR JSON PATH";

                command.Parameters.Add(new SqlParameter("@offset", (pageParameters.PageNumber - 1) * pageParameters.PageSize));
                command.Parameters.Add(new SqlParameter("@pageSize", pageParameters.PageSize));

                await this._db.Database.OpenConnectionAsync(cancellationToken);
                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        json.Append(reader.GetValue(0).ToString());
                    }
                }
            }
            var courses = JsonConvert.DeserializeObject<List<CourseStatisticsQueryModel>>(json.ToString());
            var totalCount = await this._db.Courses.CountAsync(cancellationToken);

            return new PagedList<CourseStatisticsQueryModel>(courses, pageParameters, totalCount);
        }

        public async Task<PagedList<UserStatisticsQueryModel>> GetUsersStatisticsAsync(PageParameters pageParameters, CancellationToken cancellationToken)
        {
            var json = new StringBuilder();
            using (var command = this._db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText =
                    "SELECT " +
                        "u.Id, " +
                        "u.[Name], " +
                        "u.Email, " +
                        "u.Balance, " +
                        "(SELECT COUNT(*) " +
                        "FROM dbo.UsersCourses AS uc " +
                        "WHERE uc.UserId = u.Id " +
                        ") AS BoughtCoursesCount, " +
                        "(SELECT COUNT(*) " +
                        "FROM dbo.[Certificates] AS c " +
                        "WHERE c.UserId = u.Id " +
                        ") AS CompletedCoursesCount " +
                    "FROM dbo.Users AS u " +
                    "ORDER BY BoughtCoursesCount DESC, CompletedCoursesCount DESC " +
                    "OFFSET @offset ROWS " +
                    "FETCH NEXT @pageSize ROWS ONLY " +
                    "FOR JSON PATH";

                command.Parameters.Add(new SqlParameter("@offset", (pageParameters.PageNumber - 1) * pageParameters.PageSize));
                command.Parameters.Add(new SqlParameter("@pageSize", pageParameters.PageSize));

                await this._db.Database.OpenConnectionAsync(cancellationToken);
                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        json.Append(reader.GetValue(0).ToString());
                    }
                }
            }
            var users = JsonConvert.DeserializeObject<List<UserStatisticsQueryModel>>(json.ToString());
            var totalCount = await this._db.Courses.CountAsync(cancellationToken);

            return new PagedList<UserStatisticsQueryModel>(users, pageParameters, totalCount);
        }
    }
}
