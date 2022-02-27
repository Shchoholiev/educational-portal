using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.Application.Services
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

        public async Task Add(Article article)
        {
            this._articlesRepository.Attach(article.Resource);
            await this._articlesRepository.Add(article);
        }

        public async Task Delete(int id)
        {
            var article = await this._articlesRepository.GetOne(id);
            await this._articlesRepository.Delete(article);
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            return await this._articlesRepository.GetAll(b => b.Resource);
        }

        public async Task<Article> GetOne(int id)
        {
            return await this._articlesRepository.GetOne(id, b => b.Resource);
        }

        public async Task<IEnumerable<Article>> GetPage(int pageSize, int pageNumber)
        {
            return await this._articlesRepository.GetPage(pageSize, pageNumber, b => b.Resource);
        }

        public async Task Update(Article article)
        {
            this._articlesRepository.Attach(article.Resource);
            await this._articlesRepository.Update(article);
        }

        public async Task<IEnumerable<Resource>> GetResources()
        {
            return await this._resourcesRepository.GetAll();
        }
    }
}
