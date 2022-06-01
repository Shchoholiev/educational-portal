using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Paging;
using EducationalPortal.Application.IRepositories;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using EducationalPortal.API.Mapping;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using EducationalPortal.Application.DTO.EducationalMaterials;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace EducationalPortal.API.Controllers
{
    [Authorize(Roles = "Creator")]
    [ApiController]
    [Route("api/books")]
    public class BooksController : Controller
    {
        private readonly IGenericRepository<Book> _booksRepository;

        private readonly IGenericRepository<Extension> _extensionsRepository;

        private readonly ICloudStorageService _cloudStorageService;

        private readonly Mapper _mapper = new();

        public BooksController(IGenericRepository<Book> booksRepository,
                               IGenericRepository<Extension> extensionsRepository,
                               ICloudStorageService cloudStorageService)
        {
            this._booksRepository = booksRepository;
            this._extensionsRepository = extensionsRepository;
            this._cloudStorageService = cloudStorageService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks([FromQuery]PageParameters pageParameters)
        {
            var books = await this._booksRepository.GetPageAsync(pageParameters, b => b.Authors, b => b.Extension);
            var metadata = new
            {
                books.TotalItems,
                books.PageSize,
                books.PageNumber,
                books.TotalPages,
                books.HasNextPage,
                books.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return books;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BookDTO bookDTO)
        {
            if (ModelState.IsValid)
            {
                if (await this._booksRepository.Exists(b => b.Name == bookDTO.Name))
                {
                    ModelState.AddModelError(string.Empty, "Video with this name already exists!");
                }
                else
                {
                    var regex = new Regex("[a-b0-9]*/");
                    var extensionName = regex.Split(bookDTO.File.ContentType);
                    var extension = (await this._extensionsRepository.GetAllAsync(e => e.Name == extensionName[1]))
                                                                     .FirstOrDefault();
                    var book = this._mapper.Map(bookDTO);
                    book.Extension = extension ?? new Extension { Name = extensionName[1] };

                    using (var stream = bookDTO.File.OpenReadStream())
                    {
                        book.Link = await this._cloudStorageService.UploadAsync(stream, bookDTO.File.FileName,
                                                                            bookDTO.File.ContentType, "books");
                    }

                    this._booksRepository.Attach(book);
                    await this._booksRepository.AddAsync(book);
                    return StatusCode(201);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await this._booksRepository.Exists(b => b.CoursesMaterials.Any(cm => cm.MaterialId == id)))
            {
                return BadRequest("This book is used in other courses!");
            }
            else
            {
                var book = await this._booksRepository.GetOneAsync(id);
                if (book == null)
                {
                    return NotFound();
                }

                await this._booksRepository.DeleteAsync(book);
                return NoContent();
            }
        }
    }
}
