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
            this._table = _db.Users;
        }

        public async Task AddAsync(User user, CancellationToken cancellationToken)
        {
            this._db.Attach(user);
            await this._table.AddAsync(user);
            await this.SaveAsync(cancellationToken);
        }

        public async Task UpdateAsync(User user, CancellationToken cancellationToken)
        {
            this._db.Attach(user);
            this._table.Update(user);
            await this.SaveAsync(cancellationToken);
        }

        public async Task DeleteAsync(User user, CancellationToken cancellationToken)
        {
            this._table.Remove(user);
            await this.SaveAsync(cancellationToken);
        }

        public Task<User?> GetUserAsync(string email, CancellationToken cancellationToken)
        {
            return this._table.Include(u => u.UserToken)
                              .Include(u => u.Roles)
                              .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public Task<User?> GetUserWithSkillsAsync(string email, CancellationToken cancellationToken)
        {
            return this._table
                       .Include(u => u.UsersSkills)
                            .ThenInclude(us => us.Skill)
                       .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public Task<User?> GetUserWithMaterialsAsync(string email, CancellationToken cancellationToken)
        {
            return this._table
                       .Include(u => u.Materials)
                       .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public Task<User?> GetAuthorAsync(string email, CancellationToken cancellationToken)
        {
            return this._table
                       .Include(u => u.CreatedCourses)
                       .Include(u => u.UsersSkills)
                            .ThenInclude(us => us.Skill)
                       .Include(u => u.Roles)
                       .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task AddAcquiredSkillsAsync(int courseId, string email, CancellationToken cancellationToken)
        {
            var skills = await this._db.CoursesSkills
                                   .Where(cs => cs.CourseId == courseId)
                                   .ToListAsync(cancellationToken);

            var user = await this.GetUserWithSkillsAsync(email, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            foreach (var skill in skills)
            {
                if (user.UsersSkills.Any(us => us.SkillId == skill.SkillId))
                {
                    user.UsersSkills.FirstOrDefault(us => us.SkillId == skill.SkillId).Level++;
                }
                else
                {
                    user.UsersSkills.Add(new UsersSkills
                    {
                        SkillId = skill.SkillId,
                        Level = 1,
                    });
                }
            }

            await this.UpdateAsync(user, cancellationToken);
        }

        private async Task SaveAsync(CancellationToken cancellationToken)
        {
            await this._db.SaveChangesAsync(cancellationToken);
        }
    }
}
