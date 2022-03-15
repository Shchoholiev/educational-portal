using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EducationalPortal.Infrastructure.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationContext _db;
        private readonly DbSet<User> _table;

        public UsersRepository()
        {
            this._db = new ApplicationContext();
            this._table = _db.Set<User>();
        }

        public async Task AddAsync(User user)
        {
            await this._table.AddAsync(user);
            await this.SaveAsync();
        }

        public async Task UpdateAsync(User user)
        {
            this._db.Attach(user);
            this._table.Update(user);
            await this.SaveAsync();
        }

        public async Task DeleteAsync(User user)
        {
            this._table.Remove(user);
            await this.SaveAsync();
        }

        public async Task<User?> GetUserAsync(string email)
        {
            return await this._table.Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserWithSkillsAsync(string email)
        {
            return await this._table
                             .Include(u => u.UsersSkills)
                                .ThenInclude(us => us.Skill)
                             .Where(u => u.Email == email)
                             .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UsersCourses>> GetUsersCoursesPageAsync(string email,
                                                                              int pageSize, int pageNumber)
        {
            var user = await this.GetUserAsync(email);
            return await this._db.UsersCourses.AsNoTracking()
                                              .Where(uc => uc.UserId == user.Id)
                                              .Include(uc => uc.Course)
                                              .Skip((pageNumber - 1) * pageSize)
                                              .Take(pageSize)
                                              .ToListAsync();
        }

        public async Task<int> GetUsersCoursesCountAsync(string email)
        {
            var user = await this.GetUserAsync(email);
            return await this._db.UsersCourses
                                 .Where(uc => uc.UserId == user.Id)
                                 .CountAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await this._table.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>> predicate)
        {
            return await this._table.Where(predicate).ToListAsync();
        }

        private async Task SaveAsync()
        {
            await this._db.SaveChangesAsync();
        }
    }
}
