using EducationalPortal.Application.Interfaces;
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

        private readonly IUsersRepository _usersRepository;

        private readonly ICoursesRepository _coursesRepository;

        public ShoppingCartService(IGenericRepository<CartItem> cartItemsRepository,
                                   IGenericRepository<ShoppingHistory> shoppingHistoryRepository,
                                   IUsersRepository usersRepository, ICoursesRepository coursesRepository)
        {
            this._cartItemsRepository = cartItemsRepository;
            this._shoppingHistoryRepository = shoppingHistoryRepository;
            this._usersRepository = usersRepository;
            this._coursesRepository = coursesRepository;
        }

        public async Task AddAsync(CartItem cartItem)
        {
            var user = await this._usersRepository.GetUserAsync(cartItem.User.Email);
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

        public async Task<IEnumerable<CartItem>> GetPageAsync(string userId, int pageSize, int pageNumber)
        {
            return await this._cartItemsRepository.GetPageAsync(pageSize, pageNumber, ci => ci.Course);
        }

        public async Task<int> GetCountAsync(string userEmail)
        {
            return await this._cartItemsRepository.GetCountAsync(ci => ci.User.Email == userEmail);
        }

        public async Task BuyAsync(string userEmail)
        {
            var user = await this._usersRepository.GetUserAsync(userEmail);
            var cartItems = await this._cartItemsRepository.GetAllAsync(ci => ci.User.Id == user.Id,
                                                                        c => c.Course);
            var date = DateTime.Now;

            foreach (var cartItem in cartItems)
            {
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
                    Progress = 0,
                };

                await this._usersRepository.AddUsersCourses(userCourse);

                cartItem.Course = null;
                await this._cartItemsRepository.DeleteAsync(cartItem);

                this._shoppingHistoryRepository.Attach(shoppingHistory);
                await this._shoppingHistoryRepository.AddAsync(shoppingHistory);
            }
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
            var cartItems = (await this._cartItemsRepository
                              .GetAllAsync(ci => ci.Course.Id == courseId
                                           && ci.User.Email == userEmail));
            return cartItems.Count() > 0;
        }
    }
}
