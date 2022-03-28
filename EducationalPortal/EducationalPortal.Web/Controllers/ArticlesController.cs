using EducationalPortal.Application.DTO;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Web.Mapping;
using EducationalPortal.Web.Paging;
using EducationalPortal.Web.ViewModels.CreateViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPortal.Web.Controllers
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
            var articles = await this._articlesRepository
                                     .GetPageAsync(pageParameters.PageSize, pageParameters.PageNumber,
                                                   a => a.Resource);
            var articlesCreateModels = this._mapper.Map(articles, chosenArticles);
            var totalCount = await this._articlesRepository.GetCountAsync(a => true);
            var pagedArticles = new PagedList<ArticleCreateModel>(articlesCreateModels, pageParameters, totalCount);

            return PartialView("_AddArticlesPanel", pagedArticles);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticleDTO articleDTO)
        {
            if ((await this._articlesRepository.GetAllAsync(a => a.Link == articleDTO.Link)).Count() > 0)
            {
                ModelState.AddModelError(string.Empty, "Article with this link already exists!");
                return PartialView("_CreateArticle", articleDTO);
            }
            else
            {
                var article = this._mapper.Map(articleDTO); 
                this._articlesRepository.Attach(article);
                await this._articlesRepository.AddAsync(article);
                return Json(new { success = true });
            }
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
            if ((await this._articlesRepository.GetCountAsync(a => a.CoursesMaterials
                                                              .Any(cm => cm.MaterialId == idArticles))) == 0)
            {
                var article = await this._articlesRepository.GetOneAsync(idArticles);
                await this._articlesRepository.DeleteAsync(article);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "This article is used in other courses!" });
            }
        }
    }
}
