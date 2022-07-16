using EducationalPortal.Application.Interfaces.Identity;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EducationalPortal.Infrastructure.Services.Identity
{
    public class TokensService : ITokensService
    {
        private readonly IConfiguration _configuration;

        private readonly IUsersRepository _usersRepository;

        private readonly ILogger _logger;

        public TokensService(IConfiguration configuration, IUsersRepository usersRepository,
                             ILogger<TokensService> logger)
        {
            this._configuration = configuration;
            this._usersRepository = usersRepository;
            this._logger = logger;
        }

        public async Task<TokensModel> Refresh(TokensModel tokensModel, string email, 
                                               CancellationToken cancellationToken)
        {
            var principal = this.GetPrincipalFromExpiredToken(tokensModel.AccessToken);

            var user = await this._usersRepository.GetUserAsync(email, cancellationToken);
            if (user == null || user?.UserToken?.RefreshToken != tokensModel.RefreshToken
                             || user?.UserToken?.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new SecurityTokenExpiredException();
            }

            var newAccessToken = this.GenerateAccessToken(principal.Claims);
            var newRefreshToken = this.GenerateRefreshToken();
            user.UserToken.RefreshToken = newRefreshToken;
            await this._usersRepository.UpdateAsync(user, cancellationToken);

            this._logger.LogInformation($"Refreshed user tokens.");

            return new TokensModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }


        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var tokenOptions = GetTokenOptions(claims);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            this._logger.LogInformation($"Generated new access token.");

            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                var refreshToken = Convert.ToBase64String(randomNumber);

                this._logger.LogInformation($"Generated new refresh token.");

                return refreshToken;
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, 
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("HHUHU92479-JKKNS23O")),
                ValidateLifetime = false 
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
                                            StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            this._logger.LogInformation($"Returned data from expired access token.");

            return principal;
        }

        private JwtSecurityToken GetTokenOptions(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetValue<string>("JsonWebTokenKeys:IssuerSigningKey")));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("JsonWebTokenKeys:ValidIssuer"),
                audience: _configuration.GetValue<string>("JsonWebTokenKeys:ValidAudience"),
                expires: DateTime.Now.AddMinutes(5),
                claims: claims,
                signingCredentials: signinCredentials
            );

            return tokenOptions;
        }
    }
}
