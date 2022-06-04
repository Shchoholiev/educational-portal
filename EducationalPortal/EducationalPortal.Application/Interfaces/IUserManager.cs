using EducationalPortal.Application.Models.DTO;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace EducationalPortal.Application.Interfaces
{
    public interface IUserManager
    {
        Task<IEnumerable<Claim>> SignInAsync(HttpContext httpContext, UserDto user, bool isPersistent);

        Task<IEnumerable<Claim>> AddToRoleAsync(HttpContext httpContext, string role);
    }
}
