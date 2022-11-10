using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Models.QueryModels;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using EducationalPortal.Core.Enums;
using EducationalPortal.Application.Models.LookupModels;

namespace EducationalPortal.Infrastructure.Repositories
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly ApplicationContext _db;

        private readonly DbSet<Course> _table;

        public CoursesRepository(ApplicationContext context)
        {
            this._db = context;
            this._table = this._db.Courses;
        }

        public async Task AddAsync(Course course, CancellationToken cancellationToken)
        {
            this._db.Attach(course);
            await this._table.AddAsync(course, cancellationToken);
            await this.SaveAsync(cancellationToken);
        }

        public async Task UpdateAsync(Course course, CancellationToken cancellationToken)
        {
            var coursesMaterials = this._db.CoursesMaterials.Where(cm => cm.CourseId == course.Id);
            this._db.CoursesMaterials.RemoveRange(coursesMaterials);
            var coursesSkills = this._db.CoursesSkills.Where(cm => cm.CourseId == course.Id);
            this._db.CoursesSkills.RemoveRange(coursesSkills);

            this._table.Update(course);
            await this.SaveAsync(cancellationToken);
        }

        public async Task DeleteAsync(Course course, CancellationToken cancellationToken)
        {
            this._table.Remove(course);
            await this.SaveAsync(cancellationToken);
        }

        public Task<Course?> GetCourseAsync(int id, CancellationToken cancellationToken)
        {
            return this._table.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<CourseQueryModel?> GetFullCourseAsync(int id, string userId, CancellationToken cancellationToken)
        {
            var json = new StringBuilder();
            using (var command = this._db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = 
                    "SELECT " +
                    "c.Id AS [Id], " +
                    "c.[Name] AS [Name], " +
                    "c.Thumbnail AS [Thumbnail], " +
                    "c.ShortDescription AS [ShortDescription], " +
                    "c.[Description] AS [Description], " +
                    "c.[Price] AS [Price], " +
                    "(SELECT " +
                        "s.Id, " +
                        "s.[Name], " +
                        "cs.[Level] " +
                    "FROM dbo.CoursesSkills AS cs " +
                        "JOIN dbo.Skills AS s ON s.Id = cs.SkillId " +
                        "JOIN dbo.Courses AS courses ON courses.Id = cs.CourseId " +
                    "WHERE courses.Id = c.Id " +
                    "FOR JSON PATH" +
                    ") AS Skills, " +
                    "u.[Name] AS [Author.Name], " +
                    "u.Position AS [Author.Position], " +
                    "u.Email AS [Author.Email], " +
                    "(SELECT " +
                        "m.Id, " +
                        "m.[Name], " +
                        "m.Link, " +
                        "b.PagesCount, " +
                        "b.PublicationYear, " +
                        "(SELECT " +
                            "a.Id, " +
                            "a.[FullName] " +
                        "FROM dbo.Books AS books " +
                            "LEFT JOIN dbo.AuthorBook AS ab ON ab.BooksId = books.Id " +
                            "LEFT JOIN dbo.Authors AS a ON ab.AuthorsId = a.Id " +
                        "WHERE books.Id = b.Id " +
                        "FOR JSON PATH " +
                        ") AS Authors," +
                        "e.Id AS [Extension.Id], " +
                        "e.[Name] AS [Extension.Name], " +
                        "a.PublicationDate, " +
                        "r.Id AS [Resource.Id], " +
                        "r.[Name] AS [Resource.Name], " +
                        "v.Duration, " +
                        "q.Id AS [Quality.Id], " +
                        "q.[Name] AS [Quality.Name], " +
                        "CASE WHEN LEN(@userId) > 0 " +
                        "THEN ( " +
                            "CAST( " +
                                "CASE WHEN EXISTS ( " +
                                "SELECT 1 " +
                                "FROM dbo.MaterialsBaseUser AS mbu " +
                                "WHERE mbu.UsersId = @userId AND mbu.MaterialsId = m.Id " +
                                ") " +
                                "THEN 1 " +
                                "ELSE 0 " +
                                "END " +
                            "AS BIT) " +
                        ") END AS [IsLearned] " +
                    "FROM dbo.CoursesMaterials AS cm " +
                        "JOIN dbo.Courses AS courses ON courses.Id = cm.CourseId " +
                        "JOIN dbo.Materials AS m ON m.Id = cm.MaterialId " +
                        "LEFT JOIN dbo.Books AS b ON m.Id = b.Id " +
                            "LEFT JOIN dbo.Extensions AS e ON e.Id = b.ExtensionId " +
                            "LEFT JOIN dbo.AuthorBook AS ab ON ab.BooksId = b.Id " +
                            "LEFT JOIN dbo.Authors AS authors ON ab.AuthorsId = authors.Id " +
                        "LEFT JOIN dbo.Articles AS a ON m.Id = a.Id " +
                            "LEFT JOIN dbo.Resources AS r ON r.Id = a.ResourceId " +
                        "LEFT JOIN dbo.Videos AS v ON m.Id = v.Id " +
                            "LEFT JOIN dbo.Qualities AS q ON q.Id = v.QualityId " +
                    "WHERE courses.Id = c.Id " +
                    "ORDER BY cm.[Index] " +
                    "FOR JSON PATH " +
                    ") AS Materials, " +
                    "(SELECT SUM(LearningMinutes) " +
                    "FROM dbo.Materials AS m " +
                    "JOIN dbo.CoursesMaterials AS cm ON cm.MaterialId = m.Id " +
                    "WHERE cm.CourseId = c.Id" +
                    ") AS LearningTime, " +
                    "CASE WHEN LEN(@userId) > 0 " +
                        "THEN ( " +
                            "CAST( " +
                                "CASE WHEN EXISTS ( " +
                                "SELECT 1 " +
                                "FROM dbo.UsersCourses AS uc " +
                                "WHERE uc.UserId = @userId AND uc.CourseId = c.Id " +
                                ") " +
                                "THEN 1 " +
                                "ELSE 0 " +
                                "END " +
                            "AS BIT) " +
                        ") END AS [IsBought] " +
                    "FROM dbo.Courses AS c " +
                        "JOIN dbo.Users AS u ON u.Id = c.AuthorId " +
                    "WHERE c.Id = @id " +
                    "FOR JSON PATH";

                command.CommandType = CommandType.Text;
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@userId", userId));

                await this._db.Database.OpenConnectionAsync(cancellationToken);
                using (var reader = await command.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        json.Append(reader.GetValue(0).ToString());
                    }
                }
            }
            var course = JsonConvert.DeserializeObject<List<CourseQueryModel>>(json.ToString());

            return course?.FirstOrDefault();
        }

        public async Task<PagedList<CourseShortQueryModel>> GetPageAsync(PageParameters pageParameters, 
            string userId, CancellationToken cancellationToken)
        {
            var courses = await this._table.AsNoTracking()
                                           .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                           .Take(pageParameters.PageSize)
                                           .Select(c => new CourseShortQueryModel
                                           {
                                               Id = c.Id,
                                               Name = c.Name,
                                               Thumbnail = c.Thumbnail,
                                               ShortDescription = c.ShortDescription,
                                               Price = c.Price,
                                               UpdateDateUTC = c.UpdateDateUTC,
                                               StudentsCount = c.UsersCourses.Count,
                                               IsBought = c.UsersCourses.Any(uc => uc.UserId == userId)
                                           })
                                           .ToListAsync(cancellationToken);
            var totalCount = await this._table.CountAsync(cancellationToken);

            return new PagedList<CourseShortQueryModel>(courses, pageParameters, totalCount);
        }

        public async Task<PagedList<CourseShortQueryModel>> GetPageAsync(PageParameters pageParameters, string filter,
            CoursesOrderBy orderBy, bool isAscending, string userId, CancellationToken cancellationToken)
        {
            Expression<Func<Course, bool>> predicate = c => c.Name.Contains(filter)
                       || c.ShortDescription.Contains(filter)
                       || c.Description.Contains(filter)
                       || c.Author.Name.Contains(filter);

            var query = _table.AsNoTracking()
                .Where(predicate);

            query = orderBy switch
            {
                CoursesOrderBy.Id => isAscending 
                    ? query.OrderBy(c => c.Id) 
                    : query.OrderByDescending(c => c.Id),
                CoursesOrderBy.Price => isAscending 
                    ? query.OrderBy(c => c.Price) 
                    : query.OrderByDescending(c => c.Price),
                CoursesOrderBy.UpdateDate => isAscending
                    ? query.OrderBy(c => c.UpdateDateUTC)
                    : query.OrderByDescending(c => c.UpdateDateUTC),
                CoursesOrderBy.StudentsCount => isAscending
                    ? query.OrderBy(c => c.UsersCourses.Count)
                    : query.OrderByDescending(c => c.UsersCourses.Count),
                _ => throw new NotImplementedException(),
            };

            var courses = await query
                .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                .Take(pageParameters.PageSize)
                .Select(c => new CourseShortQueryModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Thumbnail = c.Thumbnail,
                    ShortDescription = c.ShortDescription,
                    Price = c.Price,
                    UpdateDateUTC = c.UpdateDateUTC,
                    StudentsCount = c.UsersCourses.Count,
                    IsBought = c.UsersCourses.Any(uc => uc.UserId == userId)
                })
                .ToListAsync(cancellationToken);
            var totalCount = await this._table.CountAsync(predicate, cancellationToken);

            return new PagedList<CourseShortQueryModel>(courses, pageParameters, totalCount);
        }

        public Task<List<CourseLookupModel>> GetLookupModelsAsync(IEnumerable<int> skillIds, 
            IEnumerable<int> courseIds, string userId, CancellationToken cancellationToken)
        {
            return this._table
                .Where(c => c.CoursesSkills.Any(cs => skillIds.Contains(cs.SkillId) 
                                                && !courseIds.Contains(cs.CourseId))
                       && !c.UsersCourses.Any(uc => uc.UserId == userId))
                .Select(c => new CourseLookupModel
                {
                    CourseId = c.Id,
                    CoursesSkills = c.CoursesSkills.Where(cs => skillIds.Contains(cs.SkillId)).ToList(),
                }).ToListAsync(cancellationToken);
        }

        public Task<List<CourseLookupModel>> GetLookupModelsAsync(IEnumerable<int> skillIds, 
            IEnumerable<int> courseIds, IEnumerable<int> materialIds, string userId, CancellationToken cancellationToken)
        {
            return this._table
                .Where(c => c.CoursesSkills.Any(cs => skillIds.Contains(cs.SkillId) 
                                                && !courseIds.Contains(cs.CourseId))
                       && !c.UsersCourses.Any(uc => uc.UserId == userId))
                .Select(c => new CourseLookupModel
                {
                    CourseId = c.Id,
                    CoursesSkills = c.CoursesSkills.Where(cs => skillIds.Contains(cs.SkillId)).ToList(),
                    MaterialIds = c.CoursesMaterials
                        .Where(cm => !materialIds.Contains(cm.MaterialId)
                               && !cm.Material.Users.Any(u => u.Id == userId))
                        .Select(cm => cm.MaterialId).ToList(),
                    LearningTime = c.CoursesMaterials
                        .Where(cm => !materialIds.Contains(cm.MaterialId)
                               && !cm.Material.Users.Any(u => u.Id == userId))
                        .Select(cm => cm.Material)
                        .Sum(m => m.LearningMinutes),
                }).ToListAsync(cancellationToken);
        }

        public Task<List<CourseShortQueryModel>> GetCoursesAsync(IEnumerable<int> courseIds, CancellationToken cancellationToken)
        {
            return this._table
                .Where(c => courseIds.Contains(c.Id))
                .Select(c => new CourseShortQueryModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Thumbnail = c.Thumbnail,
                    ShortDescription = c.ShortDescription,
                    Price = c.Price,
                    UpdateDateUTC = c.UpdateDateUTC,
                    StudentsCount = c.UsersCourses.Count
                })
                .ToListAsync(cancellationToken);
        }

        public Task<int> GetMaterialsCountAsync(int courseId, CancellationToken cancellationToken)
        {
            return this._db.CoursesMaterials.AsNoTracking()
                                            .Where(cm => cm.CourseId == courseId)
                                            .CountAsync(cancellationToken);
        }

        public Task<User?> GetCourseAuthor(int courseId, CancellationToken cancellationToken)
        {
            return this._db.Users.FirstOrDefaultAsync(u => u.CreatedCourses.Any(c => c.Id == courseId), 
                                                      cancellationToken);
        }

        public Task<List<Skill>> GetSkillsAsync(IEnumerable<int> skillIds, CancellationToken cancellationToken)
        {
            return this._db.Skills.AsNoTracking()
                .Where(s => skillIds.Contains(s.Id))
                .ToListAsync(cancellationToken);
        }

        private async Task SaveAsync(CancellationToken cancellationToken)
        {
            await this._db.SaveChangesAsync(cancellationToken);
        }
    }
}
