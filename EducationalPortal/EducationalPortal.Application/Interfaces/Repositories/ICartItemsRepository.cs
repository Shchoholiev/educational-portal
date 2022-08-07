using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;

namespace EducationalPortal.Application.Interfaces.Repositories
{
    public interface ICartItemsRepository
    {
        Task AddAsync(CartItem cartItem, CancellationToken cancellationToken);

        Task DeleteAsync(CartItem cartItem, CancellationToken cancellationToken);

        Task<CartItem?> GetOneAsync(int id, CancellationToken cancellationToken);

        Task<PagedList<CartItem>> GetPageAsync(PageParameters pageParameters, string email, 
                                               CancellationToken cancellationToken);

        Task<List<CartItem>> GetAllAsync(string email, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(int courseId, string email, CancellationToken cancellationToken);

        Task<int> GetTotalPriceAsync(string email, CancellationToken cancellationToken);
    }
}
