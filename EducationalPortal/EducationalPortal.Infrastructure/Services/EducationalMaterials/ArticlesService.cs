using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces.EducationalMaterials;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Mapping;
using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.EducationalMaterials;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities.EducationalMaterials;
using Microsoft.Extensions.Logging;

namespace EducationalPortal.Infrastructure.Services.EducationalMaterials
{
    public class ArticlesService : IArticlesService
    {
        private readonly IGenericRepository<Article> _articlesRepository;

        private readonly ILogger _logger;

        private readonly Mapper _mapper = new();

        public ArticlesService(IGenericRepository<Article> resourcesRepository, 
                               ILogger<ArticlesService> logger)
        {
            this._articlesRepository = resourcesRepository;
            this._logger = logger;
        }

        public async Task CreateAsync(ArticleCreateDto articleDto)
        {
            var article = this._mapper.Map(articleDto);
            this._articlesRepository.Attach(article);
            await this._articlesRepository.AddAsync(article);

            this._logger.LogInformation($"Created article with id: {article.Id}.");
        }

        public async Task DeleteAsync(int id)
        {
            if (await this._articlesRepository.Exists(a => a.CoursesMaterials.Any(cm => cm.MaterialId == id)))
            {
                throw new DeleteEntityException("This article is used in other courses!");
            }

            var article = await this._articlesRepository.GetOneAsync(id);
            if (article == null)
            {
                throw new NotFoundException("Article");
            }

            await this._articlesRepository.DeleteAsync(article);

            this._logger.LogInformation($"Deleted article with id: {article.Id}.");
        }

        public async Task<PagedList<ArticleDto>> GetPageAsync(PageParameters pageParameters)
        {
            var articles = await this._articlesRepository.GetPageAsync(pageParameters);
            var dtos = this._mapper.Map(articles);

            this._logger.LogInformation($"Returned articles page {articles.PageNumber} from database.");

            return dtos;
        }
    }
}
