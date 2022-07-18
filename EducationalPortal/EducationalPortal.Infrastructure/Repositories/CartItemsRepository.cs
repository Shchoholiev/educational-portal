using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace EducationalPortal.Infrastructure.Repositories
{
    public class CartItemsRepository : ICartItemsRepository
    {
        private readonly ApplicationContext _db;

        private readonly DbSet<CartItem> _table;

        public CartItemsRepository(ApplicationContext context)
        {
            this._db = context;
            this._table = this._db.CartItems;
        }

        public async Task AddAsync(CartItem cartItem, CancellationToken cancellationToken)
        {
            this._db.Attach(cartItem);
            await this._table.AddAsync(cartItem, cancellationToken);
            await this.SaveAsync(cancellationToken);
        }

        public async Task DeleteAsync(CartItem cartItem, CancellationToken cancellationToken)
        {
            this._table.Remove(cartItem);
            await this.SaveAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync(int courseId, string email, CancellationToken cancellationToken)
        {
            return await this._table.AnyAsync(ci => ci.Course.Id == courseId && ci.User.Email == email, 
                                              cancellationToken);
        }

        public async Task<IEnumerable<CartItem>> GetAllAsync(string email, CancellationToken cancellationToken)
        {
            return await this._table.AsNoTracking()
                                    .Include(ci => ci.Course)
                                    .Where(ci => ci.User.Email == email)
                                    .ToListAsync(cancellationToken);
        }

        public async Task<CartItem?> GetOneAsync(int id, CancellationToken cancellationToken)
        {
            return await this._table.FirstOrDefaultAsync(ci => ci.Id == id, cancellationToken);
        }

        public async Task<PagedList<CartItem>> GetPageAsync(PageParameters pageParameters, string email, 
                                                            CancellationToken cancellationToken)
        {
            var cartItems = await this._table
                                     .AsNoTracking()
                                     .Include(ci => ci.Course)
                                     .Where(ci => ci.User.Email == email)
                                     .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                     .Take(pageParameters.PageSize)
                                     .ToListAsync(cancellationToken);
            var totalCount = await this._table.CountAsync(ci => ci.User.Email == email, cancellationToken);

            return new PagedList<CartItem>(cartItems, pageParameters, totalCount);
        }

        public async Task<int> GetTotalPriceAsync(string email, CancellationToken cancellationToken)
        {
            return await this._table.AsNoTracking()
                                    .Where(ci => ci.User.Email == email)
                                    .SumAsync(ci => ci.Course.Price, cancellationToken);
        }

        private async Task SaveAsync(CancellationToken cancellationToken)
        {
            await this._db.SaveChangesAsync(cancellationToken);
        }
    }
}
