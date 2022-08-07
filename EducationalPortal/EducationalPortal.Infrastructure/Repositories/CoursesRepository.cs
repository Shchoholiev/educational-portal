using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EducationalPortal.Application.Interfaces.Repositories;

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

        public async Task<Course?> GetFullCourseAsync(int id, CancellationToken cancellationToken)
        {
            var course = await this._table
               .Include(c => c.Author)
               .Include(c => c.CoursesMaterials)
                  .ThenInclude(cm => cm.Material)
               .Include(c => c.CoursesSkills)
                  .ThenInclude(cs => cs.Skill)
               .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

            if (course == null)
            {
                return null;
            }

            var materials = new List<MaterialsBase>();
            foreach (var cm in course.CoursesMaterials.OrderBy(cm => cm.Index))
            {
                var book = await this._db.Books
                    .Include(b => b.Authors)
                    .Include(b => b.Extension)
                    .FirstOrDefaultAsync(b => b.Id == cm.MaterialId, cancellationToken);

                var video = await this._db.Videos
                    .Include(v => v.Quality)
                    .FirstOrDefaultAsync(v => v.Id == cm.MaterialId, cancellationToken);

                var article = await this._db.Articles
                    .Include(a => a.Resource)
                    .FirstOrDefaultAsync(a => a.Id == cm.MaterialId, cancellationToken);

                course.CoursesMaterials[cm.Index - 1].Material = book ?? video ?? article ?? new MaterialsBase();
            }

            return course;
        }

        public async Task<PagedList<Course>> GetPageAsync(PageParameters pageParameters, 
                                                          CancellationToken cancellationToken)
        {
            var courses = await this._table.AsNoTracking()
                                           .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                           .Take(pageParameters.PageSize)
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
