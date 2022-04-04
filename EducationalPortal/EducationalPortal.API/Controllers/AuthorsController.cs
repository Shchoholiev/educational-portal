using EducationalPortal.Application.Paging;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using EducationalPortal.API.Mapping;
using EducationalPortal.API.ViewModels.CreateViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPortal.API.Controllers
{
    [Authorize(Roles = "Creator")]
    public class AuthorsController : Controller
    {
        private readonly IGenericRepository<Author> _authorsRepository;

        private readonly Mapper _mapper = new();

        public AuthorsController(IGenericRepository<Author> authorsRepository)
        {
            this._authorsRepository = authorsRepository;
        }

        [HttpGet]
        public async Task<PartialViewResult> Index(PageParameters pageParameters, List<Author> chosenAuthors)
        {
            pageParameters.PageSize = 3;
            var authors = await this._authorsRepository.GetPageAsync(pageParameters);
            var skillCreateModels = this._mapper.Map(authors, chosenAuthors);
            var totalCount = await this._authorsRepository.GetCountAsync(a => true);
            var pagedAuthors = new PagedList<AuthorCreateModel>(skillCreateModels, pageParameters, totalCount);

            return PartialView("_AddAuthors", pagedAuthors);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Author author)
        {
            if (ModelState.IsValid)
            {
                if (await this._authorsRepository.Exists(a => a.FullName == author.FullName))
                {
                    ModelState.AddModelError(string.Empty, "Author already exists!");
                }
                else
                {
                    await this._authorsRepository.AddAsync(author);
                    return Json(new { success = true });
                }
            }

            return PartialView("_CreateAuthor", author);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int idAuthors, List<Author> authors)
        {
            var author = await this._authorsRepository.GetOneAsync(idAuthors);
            authors.Add(author);
            return PartialView("_Authors", authors);
        }

        [HttpPost]
        public IActionResult Remove(int idAuthors, List<Author> authors)
        {
            authors.Remove(authors.FirstOrDefault(s => s.Id == idAuthors));
            return PartialView("_Authors", authors);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int idAuthors)
        {
            if (await this._authorsRepository.Exists(a => a.Books.Any(b => b.Authors.Any(a => a.Id == idAuthors))))
            {
                return Json(new { success = false, message = "This author is used in other books!" });
            }
            else
            {
                var author = await this._authorsRepository.GetOneAsync(idAuthors);
                await this._authorsRepository.DeleteAsync(author);
                return Json(new { success = true });
            }
        }
    }
}
