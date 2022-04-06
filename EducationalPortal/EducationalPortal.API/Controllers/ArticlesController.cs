using EducationalPortal.Application.Paging;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.API.Mapping;
using EducationalPortal.API.ViewModels.CreateViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EducationalPortal.Application.DTO.EducationalMaterials;

namespace EducationalPortal.API.Controllers
{
    [Authorize(Roles = "Creator")]
    public class ArticlesController : Controller
    {
        private readonly IGenericRepository<Article> _articlesRepository;

        private readonly Mapper _mapper = new();

        public ArticlesController(IGenericRepository<Article> articlesRepository)
        {
            this._articlesRepository = articlesRepository;
        }

        [HttpGet]
        public async Task<PartialViewResult> Index(PageParameters pageParameters, List<Article> chosenArticles)
        {
            pageParameters.PageSize = 8;
            var articles = await this._articlesRepository.GetPageAsync(pageParameters, a => a.Resource);
            var articlesCreateModels = this._mapper.Map(articles, chosenArticles);
            var totalCount = await this._articlesRepository.GetCountAsync(a => true);
            var pagedArticles = new PagedList<ArticleCreateModel>(articlesCreateModels, pageParameters, totalCount);

            return PartialView("_AddArticlesPanel", pagedArticles);
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
                    return Json(new { success = true });
                }
            }

            return PartialView("_CreateArticle", articleDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int idArticles, List<MaterialsBase> materials)
        {
            var article = await this._articlesRepository.GetOneAsync(idArticles);
            materials.Add(article);
            return PartialView("_Materials", materials);
        }

        [HttpPost]
        public IActionResult Remove(int idArticles, List<MaterialsBase> materials)
        {
            materials.Remove(materials.FirstOrDefault(m => m.Id == idArticles));
            return PartialView("_Materials", materials);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int idArticles)
        {
            if (await this._articlesRepository.Exists(a => a.CoursesMaterials.Any(cm => cm.MaterialId == idArticles)))
            {
                return Json(new { success = false, message = "This article is used in other courses!" });
            }
            else
            {
                var article = await this._articlesRepository.GetOneAsync(idArticles);
                await this._articlesRepository.DeleteAsync(article);
                return Json(new { success = true });
            }
        }
    }
}
