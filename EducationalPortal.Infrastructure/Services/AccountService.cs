using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Interfaces.Identity;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Mapping;
using EducationalPortal.Application.Models;
using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities.JoinEntities;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace EducationalPortal.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUsersRepository _usersRepository;

        private readonly IUsersCoursesRepository _usersCoursesRepository;

        private readonly IUserManager _userManager;

        private readonly IShoppingCartService _shoppingCartService;

        private readonly ILogger _logger;

        private readonly Mapper _mapper = new();

        public AccountService(IUsersRepository userRepository, IUsersCoursesRepository usersCoursesRepository,
                              IUserManager userManager, IShoppingCartService shoppingCartService,
                              ILogger<AccountService> logger)
        {
            this._usersRepository = userRepository;
            this._usersCoursesRepository = usersCoursesRepository;
            this._userManager = userManager;
            this._shoppingCartService = shoppingCartService;
            this._logger = logger;
        }

        public async Task<TokensModel> RegisterAsync(RegisterModel register, 
                                                     CancellationToken cancellationToken)
        {
            var tokens = await this._userManager.RegisterAsync(register, cancellationToken);
            await this._shoppingCartService.CheckShoppingCartCookiesAsync(register.Email, 
                register.ShoppingCart, cancellationToken);
            
            this._logger.LogInformation($"Registered user with email: {register.Email}.");

            return tokens;
        }

        public async Task<TokensModel> LoginAsync(LoginModel login, CancellationToken cancellationToken)
        {
            return await this._userManager.LoginAsync(login, cancellationToken);
        }

        public async Task<TokensModel> UpdateAsync(string email, UserDto userDto, 
                                                   CancellationToken cancellationToken)
        {
            return await this._userManager.UpdateAsync(email, userDto, cancellationToken);
        }

        public async Task DeleteAsync(string email, CancellationToken cancellationToken)
        {
            var user = await this._usersRepository.GetUserAsync(email, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            await this._usersRepository.DeleteAsync(user, cancellationToken);

            this._logger.LogInformation($"Deleted user with email: {user.Email}.");
        }

        public async Task<TokensModel> AddToRoleAsync(string roleName, string email, 
                                                      CancellationToken cancellationToken)
        {
            return await this._userManager.AddToRoleAsync(roleName, email, cancellationToken);
        }

        public async Task<UserDto> GetUserAsync(string email, CancellationToken cancellationToken)
        {
            var user = await this._usersRepository.GetAuthorAsync(email, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            var dto = this._mapper.Map(user);

            this._logger.LogInformation($"Returned user with email: {user.Email}.");

            return dto;
        }

        public async Task<UserDto> GetAuthorAsync(string email, CancellationToken cancellationToken)
        {
            var author = await this._usersRepository.GetAuthorAsync(email, cancellationToken);
            if (author == null)
            {
                throw new NotFoundException("User");
            }

            if (!author.Roles.Any(r => r.Name == "Creator"))
            {
                throw new NotFoundException("Author");
            }

            var dto = this._mapper.Map(author);

            this._logger.LogInformation($"Returned user as author with email: {author.Email}.");

            return dto;
        }

        public async Task<PagedList<UsersCourses>> GetUsersCoursesPageAsync(string email, PageParameters pageParameters, 
                                                                Expression<Func<UsersCourses, bool>> predicate, 
                                                                CancellationToken cancellationToken)
        {
            var usersCourses = await this._usersCoursesRepository
                .GetUsersCoursesPageAsync(email, pageParameters, predicate, cancellationToken);
            foreach (var uc in usersCourses)
            {
                var learnedMaterialsCount = uc.LearnedMaterialsCount;
                uc.LearnedMaterialsCount = await this._usersCoursesRepository
                    .GetLearnedMaterialsCountAsync(uc.CourseId, email, cancellationToken);
                await this._usersCoursesRepository.UpdateUsersCoursesAsync(uc, cancellationToken);
                if (learnedMaterialsCount != uc.LearnedMaterialsCount
                    && uc.LearnedMaterialsCount == uc.MaterialsCount)
                {
                    await this._usersRepository.AddAcquiredSkillsAsync(uc.CourseId, email, cancellationToken);
                    this._logger.LogInformation($"Added skills of course with id: {uc.CourseId} " +
                                            $"to user with email: {email}. Course learned.");
                }
            }

            this._logger.LogInformation($"Returned users courses page {usersCourses.PageNumber} from database.");

            return usersCourses;
        }
    }
}
