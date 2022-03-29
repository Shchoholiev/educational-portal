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
            var coursesMaterials = this._db.CoursesMaterials.Where(cm => cm.CourseId == course.Id);
            this._db.CoursesMaterials.RemoveRange(coursesMaterials);
            var coursesSkills = this._db.CoursesSkills.Where(cs => cs.CourseId == course.Id);
            this._db.CoursesSkills.RemoveRange(coursesSkills);

            try
            {
                this._table.Update(course);
            }
            catch (Exception)
            {

                throw;
            }
            
            await this.SaveAsync();
        }

        public async Task DeleteAsync(Course course)
        {
            this._table.Remove(course);
            await this.SaveAsync();
        }

        public async Task<Course?> GetCourseAsync(int id)
        {
            return await this._table.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Course> GetFullCourseAsync(int id)
        {
            var course = await this._table
               .AsNoTracking()
               .Include(c => c.Author)
               .Include(c => c.CoursesMaterials)
                  .ThenInclude(cm => cm.Material)
               .Include(c => c.CoursesSkills)
                  .ThenInclude(cs => cs.Skill)
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

            course.Skills = new List<Skill>();
            foreach (var cs in course.CoursesSkills)
            {
                course.Skills.Add(cs.Skill);
            }
            course.CoursesSkills = null;

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

        public async Task<int> GetCountAsync()
        {
            return await this._table.CountAsync();
        }

        public async Task<int> GetMaterialsCountAsync(int courseId)
        {
            return await this._db.CoursesMaterials
                                 .AsNoTracking()
                                 .Where(cm => cm.CourseId == courseId)
                                 .CountAsync();
        }

        public void Attach(params object[] obj)
        {
            foreach (var o in obj)
            {
                this._db.Attach(o);
            }
        }

        private async Task SaveAsync()
        {
            try
            {
                await this._db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var m = e.Message;
            }
        }

        public async Task<User> GetCourseAuthor(int courseId)
        {
            return await this._db.Users.FirstOrDefaultAsync(u => u.CreatedCourses.Any(c => c.Id == courseId));
        }
    }
}
