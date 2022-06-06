using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EducationalPortal.Application.Interfaces.Repositories;

namespace EducationalPortal.Infrastructure.IRepositories
{
    public class UsersCoursesRepository : IUsersCoursesRepository
    {
        private readonly ApplicationContext _db;

        private readonly DbSet<User> _table;

        public UsersCoursesRepository(ApplicationContext context)
        {
            this._db = context;
            this._table = _db.Set<User>();
        }

        public async Task<UsersCourses?> GetUsersCoursesAsync(int courseId, string email)
        {
            return await this._db.UsersCourses
                                 .FirstOrDefaultAsync(uc => uc.CourseId == courseId && uc.User.Email == email);
        }

        public async Task<PagedList<UsersCourses>> GetUsersCoursesPageAsync(string email, PageParameters pageParameters, 
                                                                            Expression<Func<UsersCourses, bool>> predicate)
        {
            var usersCourses =  await this._db.UsersCourses.AsNoTracking()
                                              .Where(uc => uc.User.Email == email)
                                              .Where(predicate)
                                              .Include(uc => uc.Course)
                                              .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                              .Take(pageParameters.PageSize)
                                              .ToListAsync();
            var totalCount = await this._db.UsersCourses
                                           .Where(uc => uc.User.Email == email)
                                           .Where(predicate)
                                           .CountAsync();

            return new PagedList<UsersCourses>(usersCourses, pageParameters, totalCount);
        }

        public async Task AddUsersCoursesAsync(UsersCourses usersCourses)
        {
            this._db.UsersCourses.Attach(usersCourses);
            await this._db.UsersCourses.AddAsync(usersCourses);
            await this.SaveAsync();
        }

        public async Task UpdateUsersCoursesAsync(UsersCourses usersCourses)
        {
            this._db.Attach(usersCourses);
            this._db.UsersCourses.Update(usersCourses);
            await this.SaveAsync();
        }

        public async Task<int> GetLearnedMaterialsCountAsync(int courseId, string email)
        {
            var user = await this._table.Include(u => u.Materials)
                                        .FirstOrDefaultAsync(u => u.Email == email);

            var courseMaterials = this._db.CoursesMaterials
                                          .AsNoTracking()
                                          .Include(cm => cm.Material)
                                          .Where(cm => cm.CourseId == courseId);
            var count = 0;
            foreach (var cm in courseMaterials)
            {
                if (user.Materials.Any(m => m.Id == cm.MaterialId))
                {
                    count++;
                }
            }

            return count;
        }

        private async Task SaveAsync()
        {
            await this._db.SaveChangesAsync();
        }
    }
}
