using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces
{
    public interface IShoppingCartService
    {
        Task AddToCartAsync(int courseId, string email);

        Task DeleteAsync(int cartItemId);

        Task<PagedList<CartItemDto>> GetPageAsync(PageParameters pageParameters, string email);

        Task<PagedList<CartItemDto>> GetPageFromCookieAsync(PageParameters pageParameters, string cookies);

        Task<int> GetTotalPriceAsync(string email);

        Task<int> GetTotalPriceFromCookieAsync(string cookies);

        Task BuyAsync(string userEmail);

        Task CheckShoppingCartCookiesAsync(string email, string? cookies);
    }
}
