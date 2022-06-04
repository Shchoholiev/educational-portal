using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.IRepositories;
using EducationalPortal.Application.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducationalPortal.API.Controllers
{
    [ApiController]
    [Route("api/token")]
    public class TokensController : Controller
    {
        private readonly ITokensService _tokenService;

        private readonly IUsersRepository _usersRepository;

        public TokensController(ITokensService tokenService, IUsersRepository usersRepository)
        {
            this._tokenService = tokenService;
            this._usersRepository = usersRepository;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokensModel tokensModel)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(tokensModel.AccessToken);
            var email = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            var user = await this._usersRepository.GetUserAsync(email);
            if (user == null || user?.UserToken?.RefreshToken != tokensModel.RefreshToken
                             || user?.UserToken?.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest();
            }

            var newAccessToken = this._tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = this._tokenService.GenerateRefreshToken();
            user.UserToken.RefreshToken = newRefreshToken;
            await this._usersRepository.UpdateAsync(user);

            return Ok(new TokensModel
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }
    }
}
