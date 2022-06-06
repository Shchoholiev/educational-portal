﻿using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EducationalPortal.Application.Interfaces.Repositories;

namespace EducationalPortal.Infrastructure.IRepositories
{
    public class CoursesRepository : ICoursesRepository
    {
        private readonly ApplicationContext _db;

        private readonly DbSet<Course> _table;

        public CoursesRepository(ApplicationContext context)
        {
            this._db = context;
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

            this._table.Update(course);
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
               .Include(c => c.Author)
               .Include(c => c.CoursesMaterials)
                  .ThenInclude(cm => cm.Material)
               .Include(c => c.Skills)
               .FirstOrDefaultAsync(c => c.Id == id);

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
                    .FirstOrDefaultAsync(b => b.Id == cm.MaterialId);

                var video = await this._db.Videos
                    .Include(v => v.Quality)
                    .FirstOrDefaultAsync(v => v.Id == cm.MaterialId);

                var article = await this._db.Articles
                    .Include(a => a.Resource)
                    .FirstOrDefaultAsync(a => a.Id == cm.MaterialId);

                course.CoursesMaterials[cm.Index - 1].Material = book ?? video ?? article ?? new MaterialsBase();
            }

            return course;
        }

        public async Task<PagedList<Course>> GetPageAsync(PageParameters pageParameters)
        {
            var courses = await this._table.AsNoTracking()
                                           .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                           .Take(pageParameters.PageSize)
                                           .ToListAsync();
            var totalCount = await this._table.CountAsync();

            return new PagedList<Course>(courses, pageParameters, totalCount);
        }

        public async Task<PagedList<Course>> GetPageAsync(PageParameters pageParameters, 
                                                          Expression<Func<Course, bool>> predicate)
        {
            var courses = await this._table.AsNoTracking()
                                           .Where(predicate)
                                           .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                           .Take(pageParameters.PageSize)
                                           .ToListAsync();
            var totalCount = await this._table.Where(predicate).CountAsync();

            return new PagedList<Course>(courses, pageParameters, totalCount);
        }

        public async Task<int> GetMaterialsCountAsync(int courseId)
        {
            return await this._db.CoursesMaterials
                                 .AsNoTracking()
                                 .Where(cm => cm.CourseId == courseId)
                                 .CountAsync();
        }

        public async Task<User> GetCourseAuthor(int courseId)
        {
            return await this._db.Users.FirstOrDefaultAsync(u => u.CreatedCourses.Any(c => c.Id == courseId));
        }

        public async Task<bool> Exists(Expression<Func<Course, bool>> predicate)
        {
            return await this._table.AnyAsync(predicate);
        }

        private async Task SaveAsync()
        {
            await this._db.SaveChangesAsync();
        }
    }
}
