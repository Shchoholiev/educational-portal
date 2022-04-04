using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Paging;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;
using System.Text.RegularExpressions;

namespace EducationalPortal.Infrastructure.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IGenericRepository<CartItem> _cartItemsRepository;

        private readonly IGenericRepository<ShoppingHistory> _shoppingHistoryRepository;

        private readonly IUsersService _usersService;

        private readonly ICoursesRepository _coursesRepository;

        public ShoppingCartService(IGenericRepository<CartItem> cartItemsRepository,
                                   IGenericRepository<ShoppingHistory> shoppingHistoryRepository,
                                   IUsersService usersService, ICoursesRepository coursesRepository)
        {
            this._cartItemsRepository = cartItemsRepository;
            this._shoppingHistoryRepository = shoppingHistoryRepository;
            this._usersService = usersService;
            this._coursesRepository = coursesRepository;
        }

        public async Task AddAsync(CartItem cartItem)
        {
            var user = await this._usersService.GetUserAsync(cartItem.User.Email);
            cartItem.Id = 0;
            cartItem.User = user;
            this._cartItemsRepository.Attach(cartItem);
            await this._cartItemsRepository.AddAsync(cartItem);
        }

        public async Task DeleteAsync(int id)
        {
            var cartItem = await this._cartItemsRepository.GetOneAsync(id);
            await this._cartItemsRepository.DeleteAsync(cartItem);
        }

        public async Task<PagedList<CartItem>> GetPageAsync(string userId, PageParameters pageParameters)
        {
            var cartItems = await this._cartItemsRepository.GetPageAsync(pageParameters, ci => ci.Course);
            var totalCount = await this._cartItemsRepository.GetCountAsync(c => true);
            return new PagedList<CartItem>(cartItems, pageParameters, totalCount);
        }

        public async Task<int> GetCountAsync(string userEmail)
        {
            return await this._cartItemsRepository.GetCountAsync(ci => ci.User.Email == userEmail);
        }

        public async Task BuyAsync(string userEmail)
        {
            var user = await this._usersService.GetUserAsync(userEmail);
            var cartItems = await this._cartItemsRepository.GetAllAsync(ci => ci.User.Id == user.Id,
                                                                        ci => ci.Course);
            var date = DateTime.Now;

            foreach (var cartItem in cartItems)
            {
                user.Balance -= cartItem.Course.Price;
                var shoppingHistory = new ShoppingHistory
                {
                    Date = date,
                    Course = cartItem.Course,
                    User = user,
                    Price = cartItem.Course.Price,
                };

                var userCourse = new UsersCourses
                {
                    Course = cartItem.Course,
                    User = user,
                    MaterialsCount = await this._coursesRepository.GetMaterialsCountAsync(cartItem.Course.Id),
                    LearnedMaterialsCount = await this._usersService.GetLearnedMaterialsCountAsync(
                                                                        cartItem.Course.Id, user.Email),
                };

                if (userCourse.MaterialsCount == userCourse.LearnedMaterialsCount)
                {
                    await this._usersService.AddAcquiredSkills(cartItem.Course.Id, userEmail);
                }

                await this._usersService.AddUsersCoursesAsync(userCourse);

                var author = await this._coursesRepository.GetCourseAuthor(cartItem.Course.Id);
                author.Balance += cartItem.Course.Price;
                await this._usersService.UpdateUserAsync(author);

                cartItem.Course = null;
                await this._cartItemsRepository.DeleteAsync(cartItem);

                this._shoppingHistoryRepository.Attach(shoppingHistory);
                await this._shoppingHistoryRepository.AddAsync(shoppingHistory);
            }

            await this._usersService.UpdateUserAsync(user);
        }

        public async Task<IEnumerable<CartItem>> GetDeserialisedAsync(string cookies)
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
                    Course = await this._coursesRepository.GetCourseAsync(courseId)
                };

                cartItems.Add(cartItem);
            }

            return cartItems;
        }

        public async Task<bool> Exists(int courseId, string userEmail)
        {
            return await this._cartItemsRepository.Exists(ci => ci.Course.Id == courseId && ci.User.Email == userEmail);
        }
    }
}
