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

        public async Task CreateAsync(ArticleCreateDto articleDto, CancellationToken cancellationToken)
        {
            var article = this._mapper.Map(articleDto);
            this._articlesRepository.Attach(article);
            await this._articlesRepository.AddAsync(article, cancellationToken);

            this._logger.LogInformation($"Created article with id: {article.Id}.");
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            if (await this._articlesRepository.ExistsAsync(
                a => a.CoursesMaterials.Any(cm => cm.MaterialId == id), cancellationToken))
            {
                throw new DeleteEntityException("This article is used in other courses!");
            }

            var article = await this._articlesRepository.GetOneAsync(id, cancellationToken);
            if (article == null)
            {
                throw new NotFoundException("Article");
            }

            await this._articlesRepository.DeleteAsync(article, cancellationToken);

            this._logger.LogInformation($"Deleted article with id: {article.Id}.");
        }

        public async Task<PagedList<ArticleDto>> GetPageAsync(PageParameters pageParameters, 
                                                              CancellationToken cancellationToken)
        {
            var articles = await this._articlesRepository.GetPageAsync(pageParameters, cancellationToken);
            var dtos = this._mapper.Map(articles);

            this._logger.LogInformation($"Returned articles page {articles.PageNumber} from database.");

            return dtos;
        }
    }
}
