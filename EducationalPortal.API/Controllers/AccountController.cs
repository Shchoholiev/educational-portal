using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Application.Models;
using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Models.UserStatistics;

namespace EducationalPortal.API.Controllers
{
    //[Authorize]
    public class AccountController : ApiControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            this._accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetProfileAsync(CancellationToken cancellationToken)
        {
            return await this._accountService.GetUserAsync(Email, cancellationToken);
        }

        [HttpGet("author/{email}")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> GetAuthorAsync(string email, CancellationToken cancellationToken)
        {
            return await this._accountService.GetAuthorAsync(email, cancellationToken);
        }

        [HttpPut]
        public async Task<ActionResult<TokensModel>> UpdateAsync([FromBody] UserDto userDto, 
                                                                 CancellationToken cancellationToken)
        {
            return await this._accountService.UpdateAsync(Email, userDto, cancellationToken);
        }

        [HttpGet("my-learning")]
        public async Task<IEnumerable<UsersCourses>> MyLearningAsync([FromQuery] PageParameters pageParameters, 
                                                                     CancellationToken cancellationToken)
        {
            var usersCourses = await this._accountService.GetUsersCoursesPageAsync(
                Email, pageParameters, uc => true, cancellationToken);
            this.SetPagingMetadata(usersCourses);
            return usersCourses;
        }

        [HttpGet("courses-in-progress")]
        public async Task<IEnumerable<UsersCourses>> CoursesInProgressAsync([FromQuery] PageParameters pageParameters, 
                                                                            CancellationToken cancellationToken)
        {
            var usersCourses = await this._accountService.GetUsersCoursesPageAsync(
                Email, pageParameters, 
                uc => uc.MaterialsCount > uc.LearnedMaterialsCount && uc.LearnedMaterialsCount > 0, 
                cancellationToken);
            this.SetPagingMetadata(usersCourses);
            return usersCourses;
        }

        [HttpGet("learned-courses")]
        public async Task<IEnumerable<UsersCourses>> LearnedCoursesAsync([FromQuery] PageParameters pageParameters, 
                                                                         CancellationToken cancellationToken)
        {
            var usersCourses = await this._accountService.GetUsersCoursesPageAsync(Email, pageParameters, 
                uc => uc.MaterialsCount == uc.LearnedMaterialsCount, cancellationToken);
            this.SetPagingMetadata(usersCourses);
            return usersCourses;
        }

        [HttpGet("learning-statistics/{dateStart}/{dateEnd}")]
        public async Task<List<LearningUserStatistics>> GetLearningStatisticsForDateRangeAsync(DateTime dateStart,
            DateTime dateEnd, CancellationToken cancellationToken)
        {
            return await _accountService.GetLearningStatisticsForDateRangeAsync(dateStart, dateEnd, UserId, cancellationToken);
        }

        [HttpGet("learning-statistics-details/{date}")]
        public async Task<DetailedLearningUserStatistics?> GetLearningStatiscsForDayAsync(DateTime date,
            CancellationToken cancellationToken)
        {
            return await _accountService.GetLearningStatiscsForDayAsync(date, UserId, cancellationToken);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<TokensModel>> RegisterAsync([FromBody] RegisterModel model, 
                                                                   CancellationToken cancellationToken)
        {
            return await this._accountService.RegisterAsync(model, cancellationToken);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<TokensModel>> LoginAsync([FromBody] LoginModel model, 
                                                                CancellationToken cancellationToken)
        {
            return await this._accountService.LoginAsync(model, cancellationToken);
        }

        [HttpPut("became-creator")]
        public async Task<ActionResult<TokensModel>> BecameCreatorAsync(CancellationToken cancellationToken)
        {
            return await this._accountService.AddToRoleAsync("Creator", Email, cancellationToken);
        }
    }
}
