using EducationalPortal.Application.Models;
using EducationalPortal.Application.Models.DTO;

namespace EducationalPortal.Application.Interfaces.Identity
{
    public interface IUserManager
    {
        Task<TokensModel> RegisterAsync(RegisterModel register);

        Task<TokensModel> LoginAsync(LoginModel login);

        Task<TokensModel> AddToRoleAsync(string roleName, string email);

        Task<TokensModel> UpdateAsync(string email, UserDto userDto);
    }
}
