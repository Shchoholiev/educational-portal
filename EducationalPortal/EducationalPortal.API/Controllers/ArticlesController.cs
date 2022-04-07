using EducationalPortal.Application.Paging;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.API.Mapping;
using EducationalPortal.API.ViewModels.CreateViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EducationalPortal.Application.DTO.EducationalMaterials;
using Newtonsoft.Json;

namespace EducationalPortal.API.Controllers
{
    //[Authorize(Roles = "Creator")]
    [ApiController]
    [Route("api/articles")]
    public class ArticlesController : Controller
    {
        private readonly IGenericRepository<Article> _articlesRepository;

        private readonly Mapper _mapper = new();

        public ArticlesController(IGenericRepository<Article> articlesRepository)
        {
            this._articlesRepository = articlesRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles([FromQuery]PageParameters pageParameters)
        {
            var articles = await this._articlesRepository.GetPageAsync(pageParameters, a => a.Resource);
            var metadata = new
            {
                articles.TotalItems,
                articles.PageSize,
                articles.PageNumber,
                articles.TotalPages,
                articles.HasNextPage,
                articles.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return articles;
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticleDTO articleDTO)
        {
            if (ModelState.IsValid)
            {
                if (await this._articlesRepository.Exists(a => a.Link == articleDTO.Link))
                {
                    ModelState.AddModelError(string.Empty, "Article with this link already exists!");
                }
                else
                {
                    var article = this._mapper.Map(articleDTO);
                    this._articlesRepository.Attach(article);
                    await this._articlesRepository.AddAsync(article);
                    return StatusCode(201);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await this._articlesRepository.Exists(a => a.CoursesMaterials.Any(cm => cm.MaterialId == id)))
            {
                return BadRequest("This article is used in other courses!");
            }
            else
            {
                var article = await this._articlesRepository.GetOneAsync(id);
                if (article == null)
                {
                    return NotFound();
                }

                await this._articlesRepository.DeleteAsync(article);
                return NoContent();
            }
        }
    }
}
