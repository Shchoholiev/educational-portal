﻿using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Interfaces.EducationalMaterials;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Mapping;
using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.EducationalMaterials;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace EducationalPortal.Infrastructure.Services.EducationalMaterials
{
    public class BooksService : IBooksService
    {
        private readonly IGenericRepository<Book> _booksRepository;

        private readonly IGenericRepository<Extension> _extensionsRepository;

        private readonly ICloudStorageService _cloudStorageService;

        private readonly ILogger _logger;

        private readonly Mapper _mapper = new();

        public BooksService(IGenericRepository<Book> resourcesRepository,
                            IGenericRepository<Extension> extensionsRepository,
                            ICloudStorageService cloudStorageService, ILogger<BooksService> logger)
        {
            this._booksRepository = resourcesRepository;
            this._extensionsRepository = extensionsRepository;
            this._cloudStorageService = cloudStorageService;
            this._logger = logger;
        }

        public async Task CreateAsync(BookCreateDto bookDto, CancellationToken cancellationToken)
        {
            var book = this._mapper.Map(bookDto);
            book.Extension = await this.GetFileExtension(bookDto.File, cancellationToken);
            using (var stream = bookDto.File.OpenReadStream())
            {
                book.Link = await this._cloudStorageService.UploadAsync(stream, bookDto.File.FileName,
                                                  bookDto.File.ContentType, "books", cancellationToken);
            }

            this._booksRepository.Attach(book);
            await this._booksRepository.AddAsync(book, cancellationToken);

            this._logger.LogInformation($"Created book with id: {book.Id}.");
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            if (await this._booksRepository.ExistsAsync(
                a => a.CoursesMaterials.Any(cm => cm.MaterialId == id), cancellationToken))
            {
                throw new DeleteEntityException("This book is used in other courses!");
            }

            var book = await this._booksRepository.GetOneAsync(id, cancellationToken);
            if (book == null)
            {
                throw new NotFoundException("Book");
            }

            await this._booksRepository.DeleteAsync(book, cancellationToken);

            this._logger.LogInformation($"Deleted book with id: {book.Id}.");
        }

        public async Task<PagedList<BookDto>> GetPageAsync(PageParameters pageParameters, 
                                                           CancellationToken cancellationToken)
        {
            var books = await this._booksRepository.GetPageAsync(pageParameters, cancellationToken, 
                                                                 b => b.Authors, b => b.Extension);
            var dtos = this._mapper.Map(books);

            this._logger.LogInformation($"Returned books page {books.PageNumber} from database.");

            return dtos;
        }

        private async Task<Extension> GetFileExtension(IFormFile file, CancellationToken cancellationToken)
        {
            var regex = new Regex("[a-b0-9]*/");
            var extensionName = regex.Split(file.ContentType);
            var extension = await this._extensionsRepository.GetOneAsync(
                e => e.Name == extensionName[1], cancellationToken);

            return extension ?? new Extension { Name = extensionName[1] };
        }
    }
}
