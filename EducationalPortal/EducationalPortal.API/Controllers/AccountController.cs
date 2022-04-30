using EducationalPortal.Application.DTO;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Paging;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;
using EducationalPortal.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EducationalPortal.API.Mapping;
using EducationalPortal.Core.Entities.JoinEntities;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace EducationalPortal.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;

        private readonly IShoppingCartService _shoppingCartService;

        private readonly IGenericRepository<Role> _rolesRepository;

        private readonly ITokenService _tokenService;

        private readonly Mapper _mapper = new();

        public AccountController(IUsersService usersService, ITokenService tokenService,
                                 IShoppingCartService shoppingCartService,
                                 IGenericRepository<Role> rolesRepository)
        {
            this._usersService = usersService;
            this._tokenService = tokenService;
            this._shoppingCartService = shoppingCartService;
            this._rolesRepository = rolesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<User>> Profile()
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await this._usersService.GetAuthorAsync(email);
            if (user == null)
            {
                return Unauthorized();
            }

            return user;
        }

        [HttpGet("author/{email}")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Author(string email)
        {
            var user = await this._usersService.GetAuthorAsync(email);
            if (user == null || !user.Roles.Any(r => r.Name == "Creator"))
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody]UserDTO userDTO)
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await this._usersService.GetUserAsync(email);
            if (user == null)
            {
                return NotFound();
            }

            if (email != userDTO.Email && await this._usersService.GetUserAsync(userDTO.Email) != null)
            {
                return BadRequest("User with this email already exists");
            }

            this._mapper.Map(user, userDTO);
            var tokens = await UpdateUserTokens(user);
            return Ok(tokens);
        }

        [HttpGet("my-learning")]
        public async Task<ActionResult<IEnumerable<UsersCourses>>> MyLearning([FromQuery]PageParameters pageParameters)
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var usersCourses = await this._usersService.GetUsersCoursesPageAsync(email, pageParameters, uc => true);
            return usersCourses;
        }

        [HttpGet("courses-in-progress")]
        public async Task<IEnumerable<UsersCourses>> CoursesInProgress([FromQuery]PageParameters pageParameters)
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var usersCourses = await this._usersService.GetUsersCoursesPageAsync(email, pageParameters, 
                    uc => uc.MaterialsCount > uc.LearnedMaterialsCount && uc.LearnedMaterialsCount > 0);
            return usersCourses;
        }

        [HttpGet("learned-courses")]
        public async Task<IEnumerable<UsersCourses>> LearnedCourses([FromQuery]PageParameters pageParameters)
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var usersCourses = await this._usersService.GetUsersCoursesPageAsync(email, pageParameters, 
                                                    uc => uc.MaterialsCount == uc.LearnedMaterialsCount);
            return usersCourses;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userDTO = new UserDTO { Name = model.Name, Email = model.Email, Password = model.Password };
                var result = await this._usersService.RegisterAsync(userDTO);
                
                if (result.Succeeded)
                {
                    var user = await this._usersService.GetUserAsync(userDTO.Email);
                    await this.CheckShoppingCartCookies(userDTO.Email);
                    var tokens = await this.UpdateUserTokens(user);
                    return Ok(tokens);
                }
                else
                {
                    return BadRequest(result.Messages);
                }
            }

            return BadRequest();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userDTO = new UserDTO { Email = model.Email, Password = model.Password };
                var result = await this._usersService.LoginAsync(userDTO);

                if (result.Succeeded)
                {
                    var user = await this._usersService.GetUserAsync(userDTO.Email);
                    var tokens = await this.UpdateUserTokens(user);
                    return Ok(tokens);
                }
                else
                {
                    return BadRequest(result.Messages);
                }
            }

            return BadRequest();
        }

        [HttpPut("became-creator")]
        public async Task<IActionResult> BecameCreator()
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var tokens = await this.AddToRole("Creator", email);

            return Ok(tokens);
        }

        private async Task<Object> AddToRole(string roleName, string email)
        {
            var role = (await this._rolesRepository.GetAllAsync(r => r.Name == roleName)).FirstOrDefault();
            var user = await this._usersService.GetUserAsync(email);
            user.Roles.Add(role);

            return await this.UpdateUserTokens(user);
        }

        private async Task<Object> UpdateUserTokens(User user)
        {
            var claims = await this.GetClaims(user);
            var accessToken = this._tokenService.GenerateAccessToken(claims);
            var refreshToken = this._tokenService.GenerateRefreshToken();

            user.UserToken = new UserToken
            {
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.Now.AddDays(7),
            };
            await this._usersService.UpdateUserAsync(user);

            return new
            {
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }

        private async Task<IEnumerable<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
            };

            foreach (var r in user.Roles)
            {
                var role = await this._rolesRepository.GetOneAsync(r.Id);
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            return claims;
        }

        private async Task CheckShoppingCartCookies(string userEmail)
        {
            var cookies = Request.Cookies["EducationalPortal_ShoppingCart"];
            if (cookies != null)
            {
                var cartItems = await this._shoppingCartService.GetDeserialisedAsync(cookies);
                foreach (var cartItem in cartItems)
                {
                    cartItem.User = new User { Email = userEmail };
                    await this._shoppingCartService.AddAsync(cartItem);
                }
                var cookieOptions = new CookieOptions() { Expires = DateTime.Now.AddDays(-1) };
                Response.Cookies.Append("EducationalPortal_ShoppingCart", "", cookieOptions);
            }
        }
    }
}
