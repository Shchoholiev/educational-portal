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
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticles([FromQuery]PageParameters pageParameters)
        {
            var articles = await this._articlesService.GetPageAsync(pageParameters);
            this.SetPagingMetadata(articles);
            return articles;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ArticleCreateDto articleDTO)
        {
            await this._articlesService.CreateAsync(articleDTO);
            return StatusCode(201);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await this._articlesService.DeleteAsync(id);
            return NoContent();
        }
    }
}
