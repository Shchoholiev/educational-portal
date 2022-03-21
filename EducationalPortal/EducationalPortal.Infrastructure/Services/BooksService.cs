using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.Infrastructure.Services
{
    public class BooksService : IBooksService
    {
        private readonly IGenericRepository<Book> _booksRepository;

        private readonly IGenericRepository<Author> _authorsRepository;

        public BooksService(IGenericRepository<Book> booksRepository,
                            IGenericRepository<Author> authorsRepository)
        {
            this._booksRepository = booksRepository;
            this._authorsRepository = authorsRepository;
        }

        public async Task AddAsync(Book book)
        {
            this._booksRepository.Attach(book.Authors);
            await this._booksRepository.AddAsync(book);
        }

        public async Task DeleteAsync(int id)
        {
            var book = await this._booksRepository.GetOneAsync(id);
            await this._booksRepository.DeleteAsync(book);
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await this._booksRepository.GetAllAsync(b => b.Authors);
        }

        public async Task<Book> GetOneAsync(int id)
        {
            return await this._booksRepository.GetOneAsync(id, b => b.Authors);
        }

        public async Task<IEnumerable<Book>> GetPageAsync(int pageSize, int pageNumber)
        {
            return await this._booksRepository.GetPageAsync(pageSize, pageNumber, b => b.Authors);
        }

        public async Task UpdateAsync(Book book)
        {
            this._booksRepository.Attach(book.Authors);
            await this._booksRepository.UpdateAsync(book);
        }

        public async Task<IEnumerable<Author>> GetAuthorsAsync()
        {
            return await this._authorsRepository.GetAllAsync();
        }
    }
}
