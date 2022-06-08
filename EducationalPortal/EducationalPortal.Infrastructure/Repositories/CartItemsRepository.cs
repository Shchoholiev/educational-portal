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
            this._table = _db.Set<CartItem>();
        }

        public async Task AddAsync(CartItem cartItem)
        {
            this._db.Attach(cartItem);
            await this._table.AddAsync(cartItem);
            await this.SaveAsync();
        }

        public async Task DeleteAsync(CartItem cartItem)
        {
            this._table.Remove(cartItem);
            await this.SaveAsync();
        }

        public async Task<bool> ExistsAsync(int courseId, string email)
        {
            return await this._table.AnyAsync(ci => ci.Course.Id == courseId && ci.User.Email == email);
        }

        public async Task<IEnumerable<CartItem>> GetAllAsync(string email)
        {
            return await this._table.AsNoTracking()
                                    .Include(ci => ci.Course)
                                    .Where(ci => ci.User.Email == email)
                                    .ToListAsync();
        }

        public async Task<CartItem?> GetOneAsync(int id)
        {
            return await this._table.FirstOrDefaultAsync(ci => ci.Id == id);
        }

        public async Task<PagedList<CartItem>> GetPageAsync(PageParameters pageParameters, string email)
        {
            var cartItems = await this._table
                                     .AsNoTracking()
                                     .Include(ci => ci.Course)
                                     .Where(ci => ci.User.Email == email)
                                     .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                     .Take(pageParameters.PageSize)
                                     .ToListAsync();
            var totalCount = await this._table.CountAsync(ci => ci.User.Email == email);

            return new PagedList<CartItem>(cartItems, pageParameters, totalCount);
        }

        public async Task<int> GetTotalPriceAsync(string email)
        {
            return await this._table.AsNoTracking()
                                    .Where(ci => ci.User.Email == email)
                                    .SumAsync(ci => ci.Course.Price);
        }

        private async Task SaveAsync()
        {
            await this._db.SaveChangesAsync();
        }
    }
}
