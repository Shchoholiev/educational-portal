using EducationalPortal.Application.Paging;
using EducationalPortal.Application.IRepositories;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EducationalPortal.Application.DTO.EducationalMaterials.Properties;
using Newtonsoft.Json;

namespace EducationalPortal.API.Controllers
{
    [Authorize(Roles = "Creator")]
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : Controller
    {
        private readonly IGenericRepository<Author> _authorsRepository;

        public AuthorsController(IGenericRepository<Author> authorsRepository)
        {
            this._authorsRepository = authorsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors([FromQuery]PageParameters pageParameters)
        {
            var authors = await this._authorsRepository.GetPageAsync(pageParameters);
            var metadata = new
            {
                authors.TotalItems,
                authors.PageSize,
                authors.PageNumber,
                authors.TotalPages,
                authors.HasNextPage,
                authors.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return authors;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]AuthorDTO authorDTO)
        {
            if (ModelState.IsValid)
            {
                if (await this._authorsRepository.Exists(a => a.FullName == authorDTO.FullName))
                {
                    ModelState.AddModelError(string.Empty, "Author already exists!");
                }
                else
                {
                    var author = new Author { FullName = authorDTO.FullName };
                    await this._authorsRepository.AddAsync(author);
                    return StatusCode(201);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await this._authorsRepository.Exists(a => a.Books.Any(b => b.Authors.Any(a => a.Id == id))))
            {
                return BadRequest("This author is used in other books!");
            }
            else
            {
                var author = await this._authorsRepository.GetOneAsync(id);
                if (author == null)
                {
                    return NotFound();
                }

                await this._authorsRepository.DeleteAsync(author);
                return NoContent();
            }
        }
    }
}
