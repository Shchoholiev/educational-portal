using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;

namespace EducationalPortal.Application.Interfaces.Repositories
{
    public interface ICartItemsRepository
    {
        Task AddAsync(CartItem cartItem);

        Task DeleteAsync(CartItem cartItem);

        Task<CartItem?> GetOneAsync(int id);

        Task<PagedList<CartItem>> GetPageAsync(PageParameters pageParameters, string email);

        Task<IEnumerable<CartItem>> GetAllAsync(string email);

        Task<bool> ExistsAsync(int courseId, string email);

        Task<int> GetTotalPriceAsync(string email);
    }
}
