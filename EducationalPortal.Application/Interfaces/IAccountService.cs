using EducationalPortal.Application.Models;
using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities.JoinEntities;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Interfaces
{
    public interface IAccountService
    {
        Task<UserDto> GetUserAsync(string email, CancellationToken cancellationToken);

        Task<UserDto> GetAuthorAsync(string email, CancellationToken cancellationToken);

        Task<TokensModel> UpdateAsync(string email, UserDto userDto, CancellationToken cancellationToken);

        Task DeleteAsync(string id, CancellationToken cancellationToken);

        Task<PagedList<UsersCourses>> GetUsersCoursesPageAsync(string email, PageParameters pageParameters,
                                                               Expression<Func<UsersCourses, bool>> predicate, 
                                                               CancellationToken cancellationToken);

        Task<TokensModel> RegisterAsync(RegisterModel register, CancellationToken cancellationToken);

        Task<TokensModel> LoginAsync(LoginModel login, CancellationToken cancellationToken);

        Task<TokensModel> AddToRoleAsync(string roleName, string email, CancellationToken cancellationToken);
    }
}
