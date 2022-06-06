using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EducationalPortal.Application.Interfaces.Repositories;

namespace EducationalPortal.Infrastructure.IRepositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ApplicationContext _db;

        private readonly DbSet<User> _table;

        public UsersRepository(ApplicationContext context)
        {
            this._db = context;
            this._table = _db.Set<User>();
        }

        public async Task AddAsync(User user)
        {
            this._db.Attach(user);
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
            return await this._table.Include(u => u.UserToken)
                                    .Include(u => u.Roles)
                                    .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserWithSkillsAsync(string email)
        {
            return await this._table
                             .Include(u => u.UsersSkills)
                                .ThenInclude(us => us.Skill)
                             .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserWithMaterialsAsync(string email)
        {
            return await this._table
                             .Include(u => u.Materials)
                             .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetAuthorAsync(string email)
        {
            return await this._table
                             .Include(u => u.CreatedCourses)
                             .Include(u => u.UsersSkills)
                                .ThenInclude(us => us.Skill)
                             .Include(u => u.Roles)
                             .FirstOrDefaultAsync(u => u.Email == email);
        }

        private async Task SaveAsync()
        {
            await this._db.SaveChangesAsync();
        }
    }
}
