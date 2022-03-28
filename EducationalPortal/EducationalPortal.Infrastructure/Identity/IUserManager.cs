using EducationalPortal.Application.DTO;
using Microsoft.AspNetCore.Http;

namespace EducationalPortal.Infrastructure.Identity
{
    public interface IUserManager
    {
        Task SignInAsync(HttpContext httpContext, UserDTO user, bool isPersistent);

        Task SignOutAsync(HttpContext httpContext);

        Task AddToRoleAsync(HttpContext httpContext, string role);
    }
}
