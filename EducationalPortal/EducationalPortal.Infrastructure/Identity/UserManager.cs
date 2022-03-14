using EducationalPortal.Application.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EducationalPortal.Infrastructure.Identity
{
    public class UserManager : IUserManager
    {
        public async Task SignInAsync(HttpContext httpContext, UserDTO user, bool isPersistent = false)
        {
            var authenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            var claims = this.GetUserClaims(user);
            var claimsIdentity = new ClaimsIdentity(claims, authenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var properties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = isPersistent,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            };

            await httpContext.SignInAsync(authenticationScheme, claimsPrincipal, properties);
        }

        public async Task SignOutAsync(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private List<Claim> GetUserClaims(UserDTO user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Name));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            return claims;
        }
    }
}
