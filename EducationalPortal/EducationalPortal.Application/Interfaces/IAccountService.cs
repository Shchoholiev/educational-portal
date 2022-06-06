using EducationalPortal.Application.Models;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Interfaces
{
    public interface IAccountService
    {
        Task<User?> GetAuthorAsync(string email);

        Task UpdateAsync(User user);

        Task DeleteAsync(string id);

        Task<PagedList<UsersCourses>> GetUsersCoursesPageAsync(string email, PageParameters pageParameters,
                                                               Expression<Func<UsersCourses, bool>> predicate);

        Task<TokensModel> RegisterAsync(RegisterModel register);

        Task<TokensModel> LoginAsync(LoginModel login);

        Task<TokensModel> AddToRoleAsync(string roleName, string email);

        Task AddAcquiredSkillsAsync(int courseId, string email);
    }
}
