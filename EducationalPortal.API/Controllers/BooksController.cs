using EducationalPortal.Application.Paging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Interfaces.EducationalMaterials;
using EducationalPortal.Application.Models.DTO.EducationalMaterials;

namespace EducationalPortal.API.Controllers
{
    [Authorize(Roles = "Creator")]
    public class BooksController : ApiControllerBase
    {
        private readonly IBooksService _booksService;

        public BooksController(IBooksService booksService)
        {
            this._booksService = booksService;
        }

        [HttpGet]
        public async Task<IEnumerable<BookDto>> GetBooksAsync([FromQuery] PageParameters pageParameters, 
                                                              CancellationToken cancellationToken)
        {
            var books = await this._booksService.GetPageAsync(pageParameters, cancellationToken);
            this.SetPagingMetadata(books);
            return books;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] BookCreateDto bookDto, 
                                                     CancellationToken cancellationToken)
        {
            await this._booksService.CreateAsync(bookDto, cancellationToken);
            return StatusCode(201);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await this._booksService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
