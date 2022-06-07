using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Application.Models;
using EducationalPortal.Application.Models.DTO;

namespace EducationalPortal.API.Controllers
{
    [Authorize]
    public class AccountController : ApiControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> Profile()
        {
            return await this._accountService.GetUserAsync(Email);
        }

        [HttpGet("author/{email}")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Author(string email)
        {
            return await this._accountService.GetAuthorAsync(email);
        }

        [HttpPut]
        public async Task<ActionResult<TokensModel>> Update([FromBody] UserDto userDto)
        {
            return await this._accountService.UpdateAsync(Email, userDto);
        }

        [HttpGet("my-learning")]
        public async Task<ActionResult<IEnumerable<UsersCourses>>> MyLearning([FromQuery] PageParameters pageParameters)
        {
            var usersCourses = await this._accountService.GetUsersCoursesPageAsync(Email, pageParameters, uc => true);
            this.SetPagingMetadata(usersCourses);
            return usersCourses;
        }

        [HttpGet("courses-in-progress")]
        public async Task<IEnumerable<UsersCourses>> CoursesInProgress([FromQuery] PageParameters pageParameters)
        {
            var usersCourses = await this._accountService.GetUsersCoursesPageAsync(Email, pageParameters, 
                    uc => uc.MaterialsCount > uc.LearnedMaterialsCount && uc.LearnedMaterialsCount > 0);
            this.SetPagingMetadata(usersCourses);
            return usersCourses;
        }

        [HttpGet("learned-courses")]
        public async Task<IEnumerable<UsersCourses>> LearnedCourses([FromQuery] PageParameters pageParameters)
        {
            var usersCourses = await this._accountService.GetUsersCoursesPageAsync(Email, pageParameters, 
                                                    uc => uc.MaterialsCount == uc.LearnedMaterialsCount);
            this.SetPagingMetadata(usersCourses);
            return usersCourses;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<TokensModel>> Register([FromBody] RegisterModel model)
        {
            return await this._accountService.RegisterAsync(model);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokensModel>> Login([FromBody] LoginModel model)
        {
            return await this._accountService.LoginAsync(model);
        }

        [HttpPut("became-creator")]
        public async Task<ActionResult<TokensModel>> BecameCreator()
        {
            return await this._accountService.AddToRoleAsync("Creator", Email);
        }
    }
}
