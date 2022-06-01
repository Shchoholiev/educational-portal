using System.Security.Claims;

namespace EducationalPortal.Application.Interfaces
{
    public interface ITokensService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
