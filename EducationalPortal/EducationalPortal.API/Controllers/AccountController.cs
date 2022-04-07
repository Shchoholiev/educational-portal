using EducationalPortal.Application.DTO;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Paging;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;
using EducationalPortal.Infrastructure.Identity;
using EducationalPortal.API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EducationalPortal.API.Mapping;
using EducationalPortal.Core.Entities.JoinEntities;

namespace EducationalPortal.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IUsersService _usersService;

        private readonly IShoppingCartService _shoppingCartService;

        private readonly IUserManager _userManager;

        private readonly IGenericRepository<Role> _rolesRepository;

        private readonly Mapper _mapper = new();

        public AccountController(IUsersService usersService, IUserManager userManager,
                                 IShoppingCartService shoppingCartService,
                                 IGenericRepository<Role> rolesRepository)
        {
            this._usersService = usersService;
            this._userManager = userManager;
            this._shoppingCartService = shoppingCartService;
            this._rolesRepository = rolesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<User>> Profile()
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await this._usersService.GetAuthorAsync(email);
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

            if (email != userDTO.Email && await this._usersService.GetUserAsync(email) != null)
            {
                return BadRequest("User with this email already exists");
            }

            this._mapper.Map(user, userDTO);
            await this._usersService.UpdateUserAsync(user);

            return NoContent();
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
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userDTO = new UserDTO { Name = model.Name, Email = model.Email, Password = model.Password };
                var result = await this._usersService.RegisterAsync(userDTO);
                
                if (result.Succeeded)
                {
                    await this._userManager.SignInAsync(this.HttpContext, userDTO, false);
                    await this.CheckShoppingCartCookies(userDTO.Email);
                    return StatusCode(201);
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userDTO = new UserDTO { Email = model.Email, Password = model.Password };
                var result = await this._usersService.LoginAsync(userDTO);

                if (result.Succeeded)
                {
                    var user = await this._usersService.GetUserAsync(userDTO.Email);
                    userDTO.Name = user.Name;
                    await this._userManager.SignInAsync(this.HttpContext, userDTO, model.RememberMe);
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Messages);
                }
            }

            return BadRequest();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await this._userManager.SignOutAsync(this.HttpContext);
            return Ok();
        }

        [HttpPut("became-creator")]
        public async Task<IActionResult> BecameCreator()
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            await this.AddToRole("Creator", email);

            return Ok();
        }

        private async Task AddToRole(string roleName, string email)
        {
            await this._userManager.AddToRoleAsync(HttpContext, roleName);

            var role = (await this._rolesRepository.GetAllAsync(r => r.Name == roleName)).FirstOrDefault();
            var user = await this._usersService.GetUserAsync(email);
            user.Roles.Add(role);
            await this._usersService.UpdateUserAsync(user);
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

        private string CheckReturnUrl(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl)
               || returnUrl.Contains("Register")
               || returnUrl.Contains("Login"))
            {
                return "/";
            }
            return returnUrl;
        }
    }
}
