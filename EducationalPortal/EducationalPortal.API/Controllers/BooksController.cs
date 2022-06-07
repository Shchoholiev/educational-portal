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
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks([FromQuery] PageParameters pageParameters)
        {
            var books = await this._booksService.GetPageAsync(pageParameters);
            this.SetPagingMetadata(books);
            return books;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BookCreateDto bookDTO)
        {
            await this._booksService.CreateAsync(bookDTO);
            return StatusCode(201);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await this._booksService.DeleteAsync(id);
            return NoContent();
        }
    }
}
