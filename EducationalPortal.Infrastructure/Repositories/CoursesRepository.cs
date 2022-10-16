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
                        "s.[Name] " +
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
                    ") AS Materials " +
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

            json.Remove(0, 1);
            json.Remove(json.Length - 3, 2);
            json.Append('}');

            var course = JsonConvert.DeserializeObject<CourseQueryModel>(json.ToString());

            return course;
        }

        public async Task<PagedList<Course>> GetPageAsync(PageParameters pageParameters, 
                                                          CancellationToken cancellationToken)
        {
            var courses = await this._table.AsNoTracking()
                                           .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                           .Take(pageParameters.PageSize)
                                           .Select(c => new Course
                                           {
                                               Id = c.Id,
                                               Name = c.Name,
                                               Thumbnail = c.Thumbnail,
                                               ShortDescription = c.ShortDescription,
                                               Price = c.Price,
                                           })
                                           .ToListAsync(cancellationToken);
            var totalCount = await this._table.CountAsync(cancellationToken);

            return new PagedList<Course>(courses, pageParameters, totalCount);
        }

        public async Task<PagedList<Course>> GetPageAsync(PageParameters pageParameters, 
                                                          Expression<Func<Course, bool>> predicate, 
                                                          CancellationToken cancellationToken)
        {
            var courses = await this._table.AsNoTracking()
                                           .Where(predicate)
                                           .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                           .Take(pageParameters.PageSize)
                                           .ToListAsync(cancellationToken);
            var totalCount = await this._table.CountAsync(predicate, cancellationToken);

            return new PagedList<Course>(courses, pageParameters, totalCount);
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

        private async Task SaveAsync(CancellationToken cancellationToken)
        {
            await this._db.SaveChangesAsync(cancellationToken);
        }
    }
}
