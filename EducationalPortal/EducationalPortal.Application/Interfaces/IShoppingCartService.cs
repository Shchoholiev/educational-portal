using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces
{
    public interface IShoppingCartService
    {
        Task AddToCartAsync(int courseId, string email, CancellationToken cancellationToken);

        Task DeleteAsync(int cartItemId, CancellationToken cancellationToken);

        Task<PagedList<CartItemDto>> GetPageAsync(PageParameters pageParameters, string email, 
                                                  CancellationToken cancellationToken);

        Task<PagedList<CartItemDto>> GetPageFromCookieAsync(PageParameters pageParameters, string cookies, 
                                                            CancellationToken cancellationToken);

        Task<int> GetTotalPriceAsync(string email, CancellationToken cancellationToken);

        Task<int> GetTotalPriceFromCookieAsync(string cookies, CancellationToken cancellationToken);

        Task BuyAsync(string userEmail, CancellationToken cancellationToken);

        Task CheckShoppingCartCookiesAsync(string email, string? cookies, CancellationToken cancellationToken);
    }
}
