using EducationalPortal.Application.Models;
using System.Security.Claims;

namespace EducationalPortal.Application.Interfaces.Identity
{
    public interface ITokensService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();

        Task<TokensModel> Refresh(TokensModel tokensModel, string email);
    }
}
