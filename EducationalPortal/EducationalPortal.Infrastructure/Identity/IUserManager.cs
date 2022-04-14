using EducationalPortal.Application.DTO;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EducationalPortal.Infrastructure.Identity
{
    public interface IUserManager
    {
        Task<IEnumerable<Claim>> SignInAsync(HttpContext httpContext, UserDTO user, bool isPersistent);

        Task SignOutAsync(HttpContext httpContext);

        Task<IEnumerable<Claim>> AddToRoleAsync(HttpContext httpContext, string role);
    }
}
