using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.Infrastructure.Services
{
    public class ArticlesService : IArticlesService
    {
        private readonly IGenericRepository<Article> _articlesRepository;

        private readonly IGenericRepository<Resource> _resourcesRepository;

        public ArticlesService(IGenericRepository<Article> articlesRepository,
                               IGenericRepository<Resource> resourcesRepository)
        {
            this._articlesRepository = articlesRepository;
            this._resourcesRepository = resourcesRepository;
        }

        public async Task AddAsync(Article article)
        {
            this._articlesRepository.Attach(article.Resource);
            await this._articlesRepository.AddAsync(article);
        }

        public async Task DeleteAsync(int id)
        {
            var article = await this._articlesRepository.GetOneAsync(id);
            await this._articlesRepository.DeleteAsync(article);
        }

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await this._articlesRepository.GetAllAsync(b => b.Resource);
        }

        public async Task<Article> GetOneAsync(int id)
        {
            return await this._articlesRepository.GetOneAsync(id, b => b.Resource);
        }

        public async Task<IEnumerable<Article>> GetPageAsync(int pageSize, int pageNumber)
        {
            return await this._articlesRepository.GetPageAsync(pageSize, pageNumber, b => b.Resource);
        }

        public async Task UpdateAsync(Article article)
        {
            this._articlesRepository.Attach(article.Resource);
            await this._articlesRepository.UpdateAsync(article);
        }

        public async Task<IEnumerable<Resource>> GetResourcesAsync()
        {
            return await this._resourcesRepository.GetAllAsync();
        }
    }
}
