using EducationalPortal.Application.Models;

namespace EducationalPortal.Application.Interfaces.Identity
{
    public interface IUserManager
    {
        Task<TokensModel> RegisterAsync(RegisterModel register);

        Task<TokensModel> LoginAsync(LoginModel login);

        Task<TokensModel> AddToRoleAsync(string roleName);
    }
}
