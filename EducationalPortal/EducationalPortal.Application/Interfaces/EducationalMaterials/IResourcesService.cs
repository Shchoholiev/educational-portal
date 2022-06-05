using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces.EducationalMaterials
{
    public interface IResourcesService
    {
        Task<PagedList<ResourceDto>> GetPageAsync(PageParameters pageParameters);

        Task Create(ResourceDto resourceDto);

        Task Delete(int id);
    }
}
