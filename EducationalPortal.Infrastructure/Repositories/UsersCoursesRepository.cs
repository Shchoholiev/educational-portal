using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EducationalPortal.Application.Interfaces.Repositories;

namespace EducationalPortal.Infrastructure.Repositories
{
    public class UsersCoursesRepository : IUsersCoursesRepository
    {
        private readonly ApplicationContext _db;

        private readonly DbSet<UsersCourses> _table;

        public UsersCoursesRepository(ApplicationContext context)
        {
            this._db = context;
            this._table = _db.UsersCourses;
        }

        public Task<UsersCourses?> GetUsersCoursesAsync(int courseId, string email, CancellationToken cancellationToken)
        {
            return this._table.FirstOrDefaultAsync(uc => uc.CourseId == courseId && uc.User.Email == email,
                                                   cancellationToken);
        }

        public async Task<PagedList<UsersCourses>> GetUsersCoursesPageAsync(string email, 
            PageParameters pageParameters, Expression<Func<UsersCourses, bool>> predicate, 
            CancellationToken cancellationToken)
        {
            var usersCourses =  await this._table.AsNoTracking()
                                                 .Where(uc => uc.User.Email == email)
                                                 .Where(predicate)
                                                 .Include(uc => uc.Course)
                                                 .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                                 .Take(pageParameters.PageSize)
                                                 .ToListAsync(cancellationToken);
            var totalCount = await this._table
                                       .Where(uc => uc.User.Email == email)
                                       .Where(predicate)
                                       .CountAsync(cancellationToken);

            return new PagedList<UsersCourses>(usersCourses, pageParameters, totalCount);
        }

        public async Task AddUsersCoursesAsync(UsersCourses usersCourses, CancellationToken cancellationToken)
        {
            this._db.UsersCourses.Attach(usersCourses);
            await this._db.UsersCourses.AddAsync(usersCourses);
            await this.SaveAsync(cancellationToken);
        }

        public async Task UpdateUsersCoursesAsync(UsersCourses usersCourses, CancellationToken cancellationToken)
        {
            this._db.Attach(usersCourses);
            this._db.UsersCourses.Update(usersCourses);
            await this.SaveAsync(cancellationToken);
        }

        public async Task<int> GetLearnedMaterialsCountAsync(int courseId, string email, 
                                                            CancellationToken cancellationToken)
        {
            return await this._db.CoursesMaterials.AsNoTracking()
                .CountAsync(cm => cm.CourseId == courseId && cm.Material.Users.Any(
                    u => u.Id == this._db.Users.FirstOrDefault(u => u.Email == email).Id));
        }

        public Task<bool> ExistsAsync(int courseId, string email, CancellationToken cancellationToken)
        {
            return this._table.AnyAsync(uc => uc.Course.Id == courseId && uc.User.Email == email, cancellationToken);
        }

        private async Task SaveAsync(CancellationToken cancellationToken)
        {
            await this._db.SaveChangesAsync(cancellationToken);
        }
    }
}
