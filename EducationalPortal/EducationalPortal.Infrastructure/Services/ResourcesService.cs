using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces.EducationalMaterials;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Mapping;
using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.Infrastructure.Services
{
    public class ResourcesService : IResourcesService
    {
        private readonly IGenericRepository<Resource> _resourcesRepository;

        private readonly Mapper _mapper = new();

        public ResourcesService(IGenericRepository<Resource> resourcesRepository)
        {
            this._resourcesRepository = resourcesRepository;
        }

        public async Task Create(ResourceDto resourceDto)
        {
            if (await this._resourcesRepository.Exists(s => s.Name == resourceDto.Name))
            {
                throw new AlreadyExistsException("resource name", resourceDto.Name);
            }

            var resource = this._mapper.Map(resourceDto);
            await this._resourcesRepository.UpdateAsync(resource);
        }

        public async Task Delete(int id)
        {
            if (await this._resourcesRepository.Exists(r => r.Articles.Any(a => a.Resource.Id == id)))
            {
                throw new DeleteEntityException("This resource is used in other courses!");
            }

            var resource = await this._resourcesRepository.GetOneAsync(id);
            if (resource == null)
            {
                throw new NotFoundException("Resource");
            }

            await this._resourcesRepository.DeleteAsync(resource);
        }

        public async Task<PagedList<ResourceDto>> GetPageAsync(PageParameters pageParameters)
        {
            var resources = await this._resourcesRepository.GetPageAsync(pageParameters);
            var dtos = this._mapper.Map(resources);
            return dtos;
        }
    }
}
