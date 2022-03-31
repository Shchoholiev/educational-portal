using EducationalPortal.Application.DTO;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using EducationalPortal.Web.Mapping;
using EducationalPortal.Web.Paging;
using EducationalPortal.Web.ViewModels.CreateViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace EducationalPortal.Web.Controllers
{
    [Authorize(Roles = "Creator")]
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
        public async Task<PartialViewResult> Index(PageParameters pageParameters, List<Book> chosenBooks)
        {
            pageParameters.PageSize = 6;
            var books = await this._booksRepository
                                  .GetPageAsync(pageParameters.PageSize, pageParameters.PageNumber,
                                                b => b.Authors, b => b.Extension);
            var booksViewModels = this._mapper.Map(books, chosenBooks);
            var totalCount = await this._booksRepository.GetCountAsync(v => true);
            var pagedVideos = new PagedList<BookCreateModel>(booksViewModels, pageParameters, totalCount);

            return PartialView("_AddBooksPanel", pagedVideos);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookDTO bookDTO)
        {
            if (ModelState.IsValid)
            {
                if ((await this._booksRepository.GetAllAsync(b => b.Name == bookDTO.Name)).Count() > 0)
                {
                    ModelState.AddModelError(string.Empty, "Video with this name already exists!");
                }
                else
                {
                    var regex = new Regex("[a-b0-9]*/");
                    var extensionName = regex.Split(bookDTO.File.ContentType);
                    var extension = (await this._extensionsRepository.GetAllAsync(e => e.Name == extensionName[1]))
                                                                     .FirstOrDefault();
                    var book = new Book
                    {
                        Name = bookDTO.Name,
                        PagesCount = bookDTO.PagesCount,
                        PublicationYear = bookDTO.PublicationYear,
                        Authors = bookDTO.Authors,
                        Extension = extension ?? new Extension { Name = extensionName[1] }
                    };

                    using (var stream = bookDTO.File.OpenReadStream())
                    {
                        book.Link = await this._cloudStorageService.UploadAsync(stream, bookDTO.File.FileName,
                                                                            bookDTO.File.ContentType, "books");
                    }

                    this._booksRepository.Attach(book);
                    await this._booksRepository.AddAsync(book);
                    return Json(new { success = true });
                }
            }

            return PartialView("_CreateBook", bookDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int idBooks, List<MaterialsBase> materials)
        {
            var book = await this._booksRepository.GetOneAsync(idBooks);
            materials.Add(book);
            return PartialView("_Materials", materials);
        }

        [HttpPost]
        public IActionResult Remove(int idBooks, List<MaterialsBase> materials)
        {
            materials.Remove(materials.FirstOrDefault(m => m.Id == idBooks));
            return PartialView("_Materials", materials);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int idBooks)
        {
            if ((await this._booksRepository.GetCountAsync(b => b.CoursesMaterials
                                                           .Any(cm => cm.MaterialId == idBooks))) == 0)
            {
                var book = await this._booksRepository.GetOneAsync(idBooks);
                await this._booksRepository.DeleteAsync(book);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "This book is used in other courses!" });
            }
        }

        [Authorize(Roles = "Student")]
        public async Task<PartialViewResult> Book(int id)
        {
            var book = await this._booksRepository.GetOneAsync(id, b => b.Extension, b => b.Authors);
            return PartialView("_Book", book);
        }
    }
}
