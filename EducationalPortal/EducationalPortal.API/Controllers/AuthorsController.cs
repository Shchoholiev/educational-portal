using EducationalPortal.Application.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using EducationalPortal.Application.Interfaces.EducationalMaterials;

namespace EducationalPortal.API.Controllers
{
    [Authorize(Roles = "Creator")]
    public class AuthorsController : ApiControllerBase
    {
        private readonly IAuthorsService _authorsService;

        public AuthorsController(IAuthorsService authorsService)
        {
            this._authorsService = authorsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors([FromQuery]PageParameters pageParameters)
        {
            var authors = await this._authorsService.GetPageAsync(pageParameters);
            this.SetPagingMetadata(authors);
            return authors;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]AuthorDto authorDto)
        {
            await this._authorsService.CreateAsync(authorDto);
            return StatusCode(201);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await this._authorsService.DeleteAsync(id);
            return NoContent();
        }
    }
}
