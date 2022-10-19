using EducationalPortal.Application.Models;
using System.Security.Claims;

namespace EducationalPortal.Application.Interfaces.Identity
{
    public interface ITokensService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();

        Task<TokensModel> RefreshAsync(TokensModel tokensModel, CancellationToken cancellationToken);
    }
}
