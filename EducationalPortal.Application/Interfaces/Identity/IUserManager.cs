using EducationalPortal.Application.Models;
using EducationalPortal.Application.Models.DTO;

namespace EducationalPortal.Application.Interfaces.Identity
{
    public interface IUserManager
    {
        Task<TokensModel> RegisterAsync(RegisterModel register, CancellationToken cancellationToken);

        Task<TokensModel> LoginAsync(LoginModel login, CancellationToken cancellationToken);

        Task<TokensModel> AddToRoleAsync(string roleName, string email, CancellationToken cancellationToken);

        Task<TokensModel> UpdateAsync(string email, UserDto userDto, CancellationToken cancellationToken);
    }
}
