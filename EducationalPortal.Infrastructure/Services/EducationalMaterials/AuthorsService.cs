﻿using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces.EducationalMaterials;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Mapping;
using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using Microsoft.Extensions.Logging;

namespace EducationalPortal.Infrastructure.Services.EducationalMaterials
{
    public class AuthorsService : IAuthorsService
    {
        private readonly IGenericRepository<Author> _authorsRepository;

        private readonly ILogger _logger;

        private readonly Mapper _mapper = new();

        public AuthorsService(IGenericRepository<Author> resourcesRepository, ILogger<AuthorsService> logger)
        {
            this._authorsRepository = resourcesRepository;
            this._logger = logger;
        }

        public async Task CreateAsync(AuthorDto authorDto, CancellationToken cancellationToken)
        {
            if (await this._authorsRepository.ExistsAsync(
                s => s.FullName == authorDto.FullName, cancellationToken))
            {
                throw new AlreadyExistsException("author full name", authorDto.FullName);
            }

            var author = this._mapper.Map(authorDto);
            await this._authorsRepository.AddAsync(author, cancellationToken);

            this._logger.LogInformation($"Created author with id: {author.Id}.");
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            if (await this._authorsRepository.ExistsAsync(
                a => a.Books.Any(b => b.Authors.Any(a => a.Id == id)), cancellationToken))
            {
                throw new DeleteEntityException("This author is used in other courses!");
            }

            var author = await this._authorsRepository.GetOneAsync(id, cancellationToken);
            if (author == null)
            {
                throw new NotFoundException("Author");
            }

            await this._authorsRepository.DeleteAsync(author, cancellationToken);

            this._logger.LogInformation($"Deleted author with id: {author.Id}.");
        }

        public async Task<PagedList<AuthorDto>> GetPageAsync(PageParameters pageParameters, 
                                                             CancellationToken cancellationToken)
        {
            var authors = await this._authorsRepository.GetPageAsync(pageParameters, cancellationToken);
            var dtos = this._mapper.Map(authors);

            this._logger.LogInformation($"Returned authors page {authors.PageNumber} from database.");

            return dtos;
        }
    }
}
