using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Mapping;
using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace EducationalPortal.Infrastructure.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ICartItemsRepository _cartItemsRepository;

        private readonly IGenericRepository<ShoppingHistory> _shoppingHistoryRepository;

        private readonly IUsersRepository _usersRepository;

        private readonly IUsersCoursesRepository _usersCoursesRepository;

        private readonly ICoursesRepository _coursesRepository;

        private readonly ILogger _logger;

        private readonly Mapper _mapper = new();

        public ShoppingCartService(ICartItemsRepository cartItemsRepository,
                                   IGenericRepository<ShoppingHistory> shoppingHistoryRepository,
                                   IUsersRepository usersRepository,
                                   IUsersCoursesRepository usersCoursesRepository, 
                                   ICoursesRepository coursesRepository, ILogger<ShoppingCartService> logger)
        {
            this._cartItemsRepository = cartItemsRepository;
            this._shoppingHistoryRepository = shoppingHistoryRepository;
            this._usersRepository = usersRepository;
            this._usersCoursesRepository = usersCoursesRepository;
            this._coursesRepository = coursesRepository;
            this._logger = logger;
        }

        public async Task AddToCartAsync(int courseId, string email, CancellationToken cancellationToken)
        {
            if (await this._cartItemsRepository.ExistsAsync(courseId, email, cancellationToken))
            {
                throw new AlreadyExistsException("Cart");
            }

            var user = await this._usersRepository.GetUserAsync(email, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            var cartItem = new CartItem
            {
                Course = new Course { Id = courseId },
                User = user,
            };

            await this._cartItemsRepository.AddAsync(cartItem, cancellationToken);

            this._logger.LogInformation($"Created cart item with id: {cartItem.Id}.");
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var cartItem = await this._cartItemsRepository.GetOneAsync(id, cancellationToken);
            if (cartItem == null)
            {
                throw new NotFoundException("Cart");
            }

            await this._cartItemsRepository.DeleteAsync(cartItem, cancellationToken);

            this._logger.LogInformation($"Deleted cart item with id: {cartItem.Id}.");
        }

        public async Task<PagedList<CartItemDto>> GetPageAsync(PageParameters pageParameters, string email, 
                                                               CancellationToken cancellationToken)
        {
            var cartItems = await this._cartItemsRepository.GetPageAsync(pageParameters, email, cancellationToken);
            var dtos = this._mapper.Map(cartItems);

            this._logger.LogInformation($"Returned cart items page {cartItems.PageNumber} from database.");

            return dtos;
        }

        public async Task<PagedList<CartItemDto>> GetPageFromCookieAsync(PageParameters pageParameters, string cookies, 
                                                                         CancellationToken cancellationToken)
        {
            var cartItems = new List<CartItem>();
            var regex = new Regex("[-]");
            var cookiesArray = regex.Split(cookies)
                                    .Where(s => !string.IsNullOrEmpty(s))
                                    .ToArray();

            for (int i = (pageParameters.PageNumber - 1) * pageParameters.PageSize; 
                 i < Math.Min(pageParameters.PageNumber * pageParameters.PageSize, cookiesArray.Length); i++)
            {
                var courseId = Convert.ToInt32(cookiesArray[i]);
                var cartItem = new CartItem
                {
                    Id = courseId,
                    Course = await this._coursesRepository.GetCourseAsync(courseId, cancellationToken)
                };

                cartItems.Add(cartItem);
            }

            var pagedList = new PagedList<CartItem>(cartItems, pageParameters, cookiesArray.Length);
            var dtos = this._mapper.Map(pagedList);

            this._logger.LogInformation($"Returned cart items page {pagedList.PageNumber} from cookies.");

            return dtos;
        }

        public async Task<int> GetTotalPriceAsync(string email, CancellationToken cancellationToken)
        {
            var totalPrice = await this._cartItemsRepository.GetTotalPriceAsync(email, cancellationToken);

            this._logger.LogInformation($"Returned total price {totalPrice} for cart from database.");

            return totalPrice;
        }

        public async Task<int> GetTotalPriceFromCookieAsync(string cookies, CancellationToken cancellationToken)
        {
            var cartItems = await this.GetAllFromCookiesAsync(cookies, cancellationToken);
            var totalPrice = cartItems.Sum(ci => ci.Course.Price);

            this._logger.LogInformation($"Returned total price {totalPrice} for cart from cookies.");

            return totalPrice;
        }

        public async Task BuyAsync(string userEmail, CancellationToken cancellationToken)
        {
            var user = await this._usersRepository.GetUserAsync(userEmail, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            var cartItems = await this._cartItemsRepository.GetAllAsync(userEmail, cancellationToken);

            await this.PayForPurchasesAsync(user, cancellationToken);
            foreach (var cartItem in cartItems)
            {
                await this.AddUsersCoursesAsync(user, cartItem.Course, cancellationToken);
                await this.AddRevenueAsync(cartItem, cancellationToken);
                await this.AddShoppingHistoryAsync(user, cartItem.Course, cancellationToken);
                await this._cartItemsRepository.DeleteAsync(cartItem, cancellationToken);
            }

            this._logger.LogInformation($"User with email {userEmail} bought courses.");
        }

        public async Task CheckShoppingCartCookiesAsync(string email, string? cookies, 
                                                        CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(cookies))
            {
                return;
            }

            var cartItems = await this.GetAllFromCookiesAsync(cookies, cancellationToken);
            foreach (var cartItem in cartItems)
            {
                if (!await this._cartItemsRepository.ExistsAsync(cartItem.Course.Id, email, cancellationToken)
                    || !await this._usersCoursesRepository.ExistsAsync(cartItem.Course.Id, email, cancellationToken))
                {
                    cartItem.User = new User { Email = email };
                    await this._cartItemsRepository.AddAsync(cartItem, cancellationToken);
                }
            }

            this._logger.LogInformation($"Added items from cookies to cart for user with email {email}.");
        }

        private async Task<IEnumerable<CartItem>> GetAllFromCookiesAsync(string cookies, 
                                                                         CancellationToken cancellationToken)
        {
            var cartItems = new List<CartItem>();
            var regex = new Regex("[-]");
            var cookiesArray = regex.Split(cookies)
                                    .Where(s => !string.IsNullOrEmpty(s));
            foreach (var cookie in cookiesArray)
            {
                var courseId = Convert.ToInt32(cookie);
                var cartItem = new CartItem
                {
                    Id = courseId,
                    Course = await this._coursesRepository.GetCourseAsync(courseId, cancellationToken)
                };

                cartItems.Add(cartItem);
            }

            this._logger.LogInformation($"Returned all cart items from cookies.");

            return cartItems;
        }

        private async Task AddShoppingHistoryAsync(User user, Course course, CancellationToken cancellationToken)
        {
            var shoppingHistory = new ShoppingHistory
            {
                Date = DateTime.Now,
                Course = course,
                User = user,
                Price = course.Price,
            };

            this._shoppingHistoryRepository.Attach(shoppingHistory);
            await this._shoppingHistoryRepository.AddAsync(shoppingHistory, cancellationToken);

            this._logger.LogInformation($"Created shopping history with id {shoppingHistory.Id}.");
        }

        private async Task AddRevenueAsync(CartItem cartItem, CancellationToken cancellationToken)
        {
            var author = await this._coursesRepository.GetCourseAuthor(cartItem.Course.Id, cancellationToken);
            author.Balance += cartItem.Course.Price;
            await this._usersRepository.UpdateAsync(author, cancellationToken);

            this._logger.LogInformation($"Added revenue to author with email {author.Email} " +
                                        $"for selling course with id: {cartItem.Course.Id}.");
        }

        private async Task PayForPurchasesAsync(User user, CancellationToken cancellationToken)
        {
            var sum = await this._cartItemsRepository.GetTotalPriceAsync(user.Email, cancellationToken);
            user.Balance -= sum;
            await this._usersRepository.UpdateAsync(user, cancellationToken);

            this._logger.LogInformation($"User with email {user.Email} payed {sum} points.");
        }

        private async Task AddUsersCoursesAsync(User user, Course course, CancellationToken cancellationToken)
        {
            var userCourse = new UsersCourses
            {
                Course = course,
                User = user,
                MaterialsCount = await this._coursesRepository.GetMaterialsCountAsync(course.Id, cancellationToken),
                LearnedMaterialsCount = await this._usersCoursesRepository.GetLearnedMaterialsCountAsync(
                                                                    course.Id, user.Email, cancellationToken),
            };

            if (userCourse.MaterialsCount == userCourse.LearnedMaterialsCount)
            {
                await this._usersRepository.AddAcquiredSkillsAsync(course.Id, user.Email, cancellationToken);
            }

            await this._usersCoursesRepository.AddUsersCoursesAsync(userCourse, cancellationToken);

            this._logger.LogInformation($"Added course with id: {course.Id} to user with email: {user.Email}.");
        }
    }
}
