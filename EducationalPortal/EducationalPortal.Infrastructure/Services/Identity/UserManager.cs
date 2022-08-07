using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces.Identity;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Mapping;
using EducationalPortal.Application.Models;
using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Core.Entities;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EducationalPortal.Infrastructure.Services.Identity
{
    public class UserManager : IUserManager
    {
        private readonly IUsersRepository _usersRepository;

        private readonly IPasswordHasher _passwordHasher;

        private readonly ITokensService _tokensService;

        private readonly IGenericRepository<Role> _rolesRepository;

        private readonly ILogger _logger;

        private readonly Mapper _mapper = new();

        public UserManager(IUsersRepository usersRepository, IPasswordHasher passwordHasher,
                           ITokensService tokensService, IGenericRepository<Role> rolesRepository,
                           ILogger<UserManager> logger)
        {
            this._usersRepository = usersRepository;
            this._passwordHasher = passwordHasher;
            this._tokensService = tokensService;
            this._rolesRepository = rolesRepository;
            this._logger = logger;
        }

        public async Task<TokensModel> RegisterAsync(RegisterModel register, CancellationToken cancellationToken)
        {
            if (await this._usersRepository.ExistsAsync(u => u.Email == register.Email, cancellationToken))
            {
                throw new AlreadyExistsException("user email", register.Email);
            }

            var role = await this._rolesRepository.GetOneAsync(r => r.Name == "Student", cancellationToken);

            var user = new User
            {
                Id = DateTime.Now.Ticks.ToString(),
                Name = register.Name,
                Email = register.Email,
                Avatar = "https://educationalportal.blob.core.windows.net/avatars/profile_default.jpg",
                Roles = new List<Role> { role },
                PasswordHash = this._passwordHasher.Hash(register.Password),
                UserToken = this.GetRefreshToken(),
            };
            
            await this._usersRepository.AddAsync(user, cancellationToken);
            var tokens = this.GetUserTokens(user);

            this._logger.LogInformation($"Created user with email: {user.Email}.");

            return tokens;
        }

        public async Task<TokensModel> LoginAsync(LoginModel login, CancellationToken cancellationToken)
        {
            var user = await this._usersRepository.GetUserAsync(login.Email, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            if (!this._passwordHasher.Check(login.Password, user.PasswordHash))
            {
                throw new InvalidDataException("Invalid password!");
            }

            user.UserToken = this.GetRefreshToken();
            await this._usersRepository.UpdateAsync(user, cancellationToken);
            var tokens = this.GetUserTokens(user);

            this._logger.LogInformation($"Logged in user with email: {login.Email}.");

            return tokens;
        }

        public async Task<TokensModel> AddToRoleAsync(string roleName, string email, 
                                                      CancellationToken cancellationToken)
        {
            var role = await this._rolesRepository.GetOneAsync(r => r.Name == roleName, cancellationToken);
            if (role == null)
            {
                throw new NotFoundException("Role");
            }

            var user = await this._usersRepository.GetUserAsync(email, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            user.Roles.Add(role);
            await this._usersRepository.UpdateAsync(user, cancellationToken);
            var tokens = this.GetUserTokens(user);

            this._logger.LogInformation($"Added role {roleName} to user with email: {email}.");

            return tokens;
        }

        public async Task<TokensModel> UpdateAsync(string email, UserDto userDto, CancellationToken cancellationToken)
        {
            var user = await this._usersRepository.GetAuthorAsync(email, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            if (email != userDto.Email 
                && await this._usersRepository.GetUserAsync(userDto.Email, cancellationToken) != null)
            {
                throw new AlreadyExistsException("email", userDto.Email);
            }

            this._mapper.Map(user, userDto);
            user.UserToken = this.GetRefreshToken();
            await this._usersRepository.UpdateAsync(user, cancellationToken);
            var tokens = this.GetUserTokens(user);

            this._logger.LogInformation($"Update user with email: {email}.");

            return tokens;
        }

        private UserToken GetRefreshToken()
        {
            var refreshToken = this._tokensService.GenerateRefreshToken();
            var token = new UserToken
            {
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.Now.AddDays(7),
            };

            this._logger.LogInformation($"Returned new refresh token.");

            return token;
        }

        private TokensModel GetUserTokens(User user)
        {
            var claims = this.GetClaims(user);
            var accessToken = this._tokensService.GenerateAccessToken(claims);

            this._logger.LogInformation($"Returned new access and refresh tokens.");

            return new TokensModel
            {
                AccessToken = accessToken,
                RefreshToken = user.UserToken.RefreshToken
            };
        }

        private IEnumerable<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            this._logger.LogInformation($"Returned claims for user with email: {user.Email}.");

            return claims;
        }
    }
}
