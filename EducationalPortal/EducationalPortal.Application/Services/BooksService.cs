using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.Application.Services
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

        public async Task Add(Book book)
        {
            this._booksRepository.Attach(book.Authors);
            await this._booksRepository.AddAsync(book);
        }

        public async Task Delete(int id)
        {
            var book = await this._booksRepository.GetOneAsync(id);
            await this._booksRepository.DeleteAsync(book);
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            return await this._booksRepository.GetAllAsync(b => b.Authors);
        }

        public async Task<Book> GetOne(int id)
        {
            return await this._booksRepository.GetOneAsync(id, b => b.Authors);
        }

        public async Task<IEnumerable<Book>> GetPage(int pageSize, int pageNumber)
        {
            return await this._booksRepository.GetPageAsync(pageSize, pageNumber, b => b.Authors);
        }

        public async Task Update(Book book)
        {
            this._booksRepository.Attach(book.Authors);
            await this._booksRepository.UpdateAsync(book);
        }

        public async Task<IEnumerable<Author>> GetAuthors()
        {
            return await this._authorsRepository.GetAllAsync();
        }
    }
}
