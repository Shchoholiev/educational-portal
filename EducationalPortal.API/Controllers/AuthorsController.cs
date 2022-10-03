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
        public async Task<IEnumerable<AuthorDto>> GetAuthorsAsync([FromQuery] PageParameters pageParameters, 
                                                                  CancellationToken cancellationToken)
        {
            var authors = await this._authorsService.GetPageAsync(pageParameters, cancellationToken);
            this.SetPagingMetadata(authors);
            return authors;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody]AuthorDto authorDto, 
                                                     CancellationToken cancellationToken)
        {
            await this._authorsService.CreateAsync(authorDto, cancellationToken);
            return StatusCode(201);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await this._authorsService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
