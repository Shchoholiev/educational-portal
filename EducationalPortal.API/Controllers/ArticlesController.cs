using EducationalPortal.Application.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Interfaces.EducationalMaterials;
using EducationalPortal.Application.Models.DTO.EducationalMaterials;

namespace EducationalPortal.API.Controllers
{
    [Authorize(Roles = "Creator")]
    public class ArticlesController : ApiControllerBase
    {
        private readonly IArticlesService _articlesService;

        public ArticlesController(IArticlesService articlesService)
        {
            this._articlesService = articlesService;
        }

        [HttpGet]
        public async Task<IEnumerable<ArticleDto>> GetArticlesAsync([FromQuery]PageParameters pageParameters, 
                                                                    CancellationToken cancellationToken)
        {
            var articles = await this._articlesService.GetPageAsync(pageParameters, cancellationToken);
            this.SetPagingMetadata(articles);
            return articles;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ArticleCreateDto articleDto, 
                                                     CancellationToken cancellationToken)
        {
            await this._articlesService.CreateAsync(articleDto, cancellationToken);
            return StatusCode(201);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await this._articlesService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
