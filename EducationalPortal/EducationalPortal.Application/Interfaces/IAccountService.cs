using EducationalPortal.Application.Models;
using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Interfaces
{
    public interface IAccountService
    {
        Task<UserDto> GetUserAsync(string email);

        Task<UserDto> GetAuthorAsync(string email);

        Task<TokensModel> UpdateAsync(string email, UserDto userDto);

        Task DeleteAsync(string id);

        Task<PagedList<UsersCourses>> GetUsersCoursesPageAsync(string email, PageParameters pageParameters,
                                                               Expression<Func<UsersCourses, bool>> predicate);

        Task<TokensModel> RegisterAsync(RegisterModel register);

        Task<TokensModel> LoginAsync(LoginModel login);

        Task<TokensModel> AddToRoleAsync(string roleName, string email);
    }
}
