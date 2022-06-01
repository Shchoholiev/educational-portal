using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;

namespace EducationalPortal.Application.Interfaces
{
    public interface IShoppingCartService
    {
        Task AddAsync(CartItem cartItem);

        Task DeleteAsync(int cartItemId);

        Task<IEnumerable<CartItem>> GetDeserialisedAsync(string cookies);

        Task<PagedList<CartItem>> GetPageAsync(string email, PageParameters pageParameters);

        Task<int> GetTotalPrice(string email);

        Task BuyAsync(string userEmail);

        Task<bool> Exists(int courseId, string userEmail);
    }
}
