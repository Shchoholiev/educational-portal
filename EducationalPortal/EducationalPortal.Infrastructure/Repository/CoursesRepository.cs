using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EducationalPortal.Infrastructure.Repository
{
    public class CoursesRepository : IGenericRepository<Course>
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

        public async Task<Course> GetOneAsync(int? id)
        {
            //return await this._table
            //    .Include(c => c.CoursesMaterials.Select(cm => cm.Material))
            //        .ThenInclude(m => m)
            //    .FirstOrDefaultAsync(c => c.Id == id);
            throw new NotImplementedException();
        }

        public Task<Course> GetOneAsync(int? id, params Expression<Func<Course, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Course>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Course>> GetAllAsync(params Expression<Func<Course, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Course>> GetAllAsync(Expression<Func<Course, bool>> predicate, params Expression<Func<Course, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Course>> GetPageAsync(int pageSize, int pageNumber)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Course>> GetPageAsync(int pageSize, int pageNumber, params Expression<Func<Course, object>>[] includeProperties)
        {
            throw new NotImplementedException();
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
            await this._db.SaveChangesAsync();
        }
    }
}
