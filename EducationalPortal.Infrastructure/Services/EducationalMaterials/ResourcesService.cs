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
    public class ResourcesService : IResourcesService
    {
        private readonly IGenericRepository<Resource> _resourcesRepository;

        private readonly ILogger _logger;

        private readonly Mapper _mapper = new();

        public ResourcesService(IGenericRepository<Resource> resourcesRepository, 
                                ILogger<ResourcesService> logger)
        {
            this._resourcesRepository = resourcesRepository;
            this._logger = logger;
        }

        public async Task CreateAsync(ResourceDto resourceDto, CancellationToken cancellationToken)
        {
            if (await this._resourcesRepository.ExistsAsync(s => s.Name == resourceDto.Name, cancellationToken))
            {
                throw new AlreadyExistsException("resource name", resourceDto.Name);
            }

            var resource = this._mapper.Map(resourceDto);
            await this._resourcesRepository.AddAsync(resource, cancellationToken);

            this._logger.LogInformation($"Created resource with id: {resource.Id}.");
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            if (await this._resourcesRepository.ExistsAsync(
                r => r.Articles.Any(a => a.Resource.Id == id), cancellationToken))
            {
                throw new DeleteEntityException("This resource is used in other courses!");
            }

            var resource = await this._resourcesRepository.GetOneAsync(id, cancellationToken);
            if (resource == null)
            {
                throw new NotFoundException("Resource");
            }

            await this._resourcesRepository.DeleteAsync(resource, cancellationToken);

            this._logger.LogInformation($"Deleted resource with id: {resource.Id}.");
        }

        public async Task<PagedList<ResourceDto>> GetPageAsync(PageParameters pageParameters, 
                                                               CancellationToken cancellationToken)
        {
            var resources = await this._resourcesRepository.GetPageAsync(pageParameters, cancellationToken);
            var dtos = this._mapper.Map(resources);

            this._logger.LogInformation($"Returned resources page {resources.PageNumber} from database.");

            return dtos;
        }
    }
}
