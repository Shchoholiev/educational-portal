using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces.Identity;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Models;
using EducationalPortal.Core.Entities;
using System.Security.Claims;

namespace EducationalPortal.Infrastructure.Services.Identity
{
    public class UserManager : IUserManager
    {
        private readonly IUsersRepository _usersRepository;

        private readonly IPasswordHasher _passwordHasher;

        private readonly ITokensService _tokensService;

        private readonly IGenericRepository<Role> _rolesRepository;

        public UserManager(IUsersRepository usersRepository, IPasswordHasher passwordHasher,
                           ITokensService tokensService, IGenericRepository<Role> rolesRepository)
        {
            this._usersRepository = usersRepository;
            this._passwordHasher = passwordHasher;
            this._tokensService = tokensService;
            this._rolesRepository = rolesRepository;
        }

        public async Task<TokensModel> RegisterAsync(RegisterModel register)
        {
            if (await this._usersRepository.GetUserAsync(register.Email) != null)
            {
                throw new AlreadyExistsException("user email", register.Email);
            }

            var role = await this._rolesRepository.GetOneAsync(r => r.Name == "Student");

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
            
            await this._usersRepository.AddAsync(user);
            var tokens = this.GetUserTokens(user);

            return tokens;
        }

        public async Task<TokensModel> LoginAsync(LoginModel login)
        {
            var user = await this._usersRepository.GetUserAsync(login.Email);

            if (user == null)
            {
                throw new InvalidDataException("User with this email was not found.");
            }

            if (!this._passwordHasher.Check(login.Password, user.PasswordHash))
            {
                throw new InvalidDataException("Invalid password!");
            }

            user.UserToken = this.GetRefreshToken();
            await this._usersRepository.UpdateAsync(user);
            var tokens = this.GetUserTokens(user);

            return tokens;
        }

        public async Task<TokensModel> AddToRoleAsync(string roleName, string email)
        {
            var role = await this._rolesRepository.GetOneAsync(r => r.Name == roleName);
            if (role == null)
            {
                throw new AlreadyExistsException("role name", roleName);
            }

            var user = await this._usersRepository.GetUserAsync(email);
            if (user == null)
            {
                throw new AlreadyExistsException("user email", email);
            }

            user.Roles.Add(role);
            await this._usersRepository.UpdateAsync(user);
            var tokens = this.GetUserTokens(user);

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

            return token;
        }

        private TokensModel GetUserTokens(User user)
        {
            var claims = this.GetClaims(user);
            var accessToken = this._tokensService.GenerateAccessToken(claims);

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

            return claims;
        }
    }
}
