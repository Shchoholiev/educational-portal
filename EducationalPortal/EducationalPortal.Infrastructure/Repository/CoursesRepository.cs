using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EducationalPortal.Infrastructure.Repository
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly ApplicationContext _db;
        private readonly DbSet<Course> _table;

        public CoursesRepository()
        {
            this._db = new ApplicationContext();
            this._table = _db.Set<Course>();
        }

        public async Task AddAsync(Course course)
        {
            this._db.Attach(course);
            await this._table.AddAsync(course);
            await this.SaveAsync();
        }

        public async Task UpdateAsync(Course course)
        {
            this._table.Update(course);
            await this.SaveAsync();
        }

        public async Task DeleteAsync(Course course)
        {
            this._table.Remove(course);
            await this.SaveAsync();
        }
        
        public async Task<Course> GetCourseAsync(int id)
        {
            var course = await this._table
               .AsNoTracking()
               .Include(c => c.Skills)
               .Include(c => c.CoursesMaterials)
                   .ThenInclude(cm => cm.Material)
               .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return null;
            }

            var materials = new List<MaterialsBase>();
            foreach (var cm in course.CoursesMaterials.OrderBy(cm => cm.Index))
            {
                var book = await this._db.Books
                    .AsNoTracking()
                    .Include(b => b.Authors)
                    .Include(b => b.Extension)
                    .FirstOrDefaultAsync(b => b.Id == cm.MaterialId);

                var video = await this._db.Videos
                    .AsNoTracking()
                    .Include(v => v.Quality)
                    .FirstOrDefaultAsync(v => v.Id == cm.MaterialId);

                var article = await this._db.Articles
                    .AsNoTracking()
                    .Include(a => a.Resource)
                    .FirstOrDefaultAsync(a => a.Id == cm.MaterialId);

                materials.Add(book ?? video ?? article ?? new MaterialsBase());
            }
            course.Materials = materials;
            course.CoursesMaterials = null;

            return course;
        }

        public async Task<IEnumerable<Course>> GetPageAsync(int pageSize, int pageNumber)
        {
            var courses = this._table.AsNoTracking()
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize);
            return await courses.ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetPageAsync(int pageSize, int pageNumber, 
                                                      Expression<Func<Course, bool>> predicate)
        {
            var courses = this._table.AsNoTracking()
                                     .Where(predicate)
                                     .Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize);
            return await courses.ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetUsersCoursesAsync(string userId)
        {
            return await this._db.UsersCourses.AsNoTracking()
                .Where(uc => uc.UserId == userId)
                .Include(uc => uc.Course)
                    .ThenInclude(c => c.UsersCourses)
                .Select(uc => uc.Course)
                .ToListAsync();
        }

        public void Attach(params object[] obj)
        {
            foreach (var o in obj)
            {
                this._db.Attach(o);
            }
        }

        public async Task<int> GetCountAsync()
        {
            return await this._table.CountAsync();
        }

        private async Task SaveAsync()
        {
            await this._db.SaveChangesAsync();
        }

        public async void GetOneAsync()
        {
            //var querry =
            //    "SELECT " +
            //    "Courses.Id AS [Id], " +
            //    "Courses.[Name] AS [Name], " +
            //    "Thumbnail, " +
            //    "[Description], " +
            //    "Price, " +
            //    "Skills.[Id] AS SkillsId, " +
            //    "Skills.[Name] AS Skills, " +
            //    "Materials.[Id] AS MaterialId, " +
            //    "Materials.[Name] AS MaterialName, " +
            //    "Materials.[Link] AS Link, " +
            //    "Videos.[Id] AS VideoId, " +
            //    "Videos.[Duration] AS Duration, " +
            //    "Qualities.[Id] AS QualityId, " +
            //    "Qualities.[Name] AS QualityName, " +
            //    "Books.[Id] AS BookId, " +
            //    "Books.[PagesCount] AS PagesCount " +
            //    "FROM dbo.[Courses] " +
            //    "LEFT JOIN dbo.[CourseSkill] ON CourseSkill.CoursesId = Courses.Id " +
            //    "LEFT JOIN dbo.[Skills] ON CourseSkill.SkillsId = Skills.Id " +
            //    "LEFT JOIN dbo.[CoursesMaterials] ON CoursesMaterials.CourseId = Courses.Id " +
            //    "LEFT JOIN dbo.[Materials] ON CoursesMaterials.MaterialId = Materials.Id " +
            //    "LEFT JOIN dbo.[Articles] ON Materials.Id = Articles.Id " +
            //    "LEFT JOIN dbo.[Resources] ON Courses.Id = Resources.Id " +
            //    "LEFT JOIN dbo.[Books] ON Materials.Id = Books.Id " +
            //    "LEFT JOIN dbo.[Extensions] ON Books.ExtensionId = Extensions.Id " +
            //    "LEFT JOIN dbo.[AuthorBook] ON AuthorBook.BooksId = Books.Id " +
            //    "LEFT JOIN dbo.[Authors] ON AuthorBook.AuthorsId = Authors.Id " +
            //    "LEFT JOIN dbo.[Videos] ON Materials.Id = Videos.Id " +
            //    "LEFT JOIN dbo.[Qualities] ON Videos.QualityId = Qualities.Id " +
            //    $"WHERE Courses.Id = {id}";

            //var course = new Course();

            //using (this._db)
            //{
            //    try
            //    {
            //        await this._db.Database.OpenConnectionAsync();
            //        var command = this._db.Database.GetDbConnection().CreateCommand();
            //        command.CommandText = querry;

            //        var reader = await command.ExecuteReaderAsync();

            //        if (reader.HasRows)
            //        {
            //            while (await reader.ReadAsync())
            //            {
            //                course.Id = reader.GetInt32(0);
            //                course.Name = reader["Name"].ToString();
            //                course.Thumbnail = reader["Thumbnail"].ToString();
            //                course.Description = reader["Description"].ToString();
            //                course.Price = (int)reader["Price"];
            //                var skill = new Skill
            //                {
            //                    Id = (int)reader["SkillId"],
            //                    Name = reader["SkillName"].ToString()
            //                };
            //            }
            //        }

            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine(e.Message);
            //    }
            //    finally
            //    {
            //        await this._db.Database.CloseConnectionAsync();
            //    }
            //}
        } // delete
    }
}
