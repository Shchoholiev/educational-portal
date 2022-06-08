using EducationalPortal.Core.Entities;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Exceptions;
using EducationalPortal.Core.Entities.JoinEntities;

namespace EducationalPortal.Infrastructure.Repositories
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

        public async Task AddAcquiredSkillsAsync(int courseId, string email)
        {
            var course = await this._db.Courses.Include(c => c.CoursesSkills)
                                                    .ThenInclude(cs => cs.Skill)
                                                .FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null)
            {
                throw new NotFoundException("Course");
            }

            var user = await this.GetUserWithSkillsAsync(email);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            foreach (var skill in course.CoursesSkills.Select(cs => cs.Skill))
            {
                if (user.UsersSkills.Any(us => us.SkillId == skill.Id))
                {
                    user.UsersSkills.FirstOrDefault(us => us.SkillId == skill.Id).Level++;
                }
                else
                {
                    user.UsersSkills.Add(new UsersSkills
                    {
                        SkillId = skill.Id,
                        Level = 1,
                    });
                }
            }

            await this.UpdateAsync(user);
        }

        private async Task SaveAsync()
        {
            await this._db.SaveChangesAsync();
        }
    }
}
