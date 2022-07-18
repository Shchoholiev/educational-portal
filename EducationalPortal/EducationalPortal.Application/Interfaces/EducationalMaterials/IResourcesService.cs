using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces.EducationalMaterials
{
    public interface IResourcesService
    {
        Task<PagedList<ResourceDto>> GetPageAsync(PageParameters pageParameters, 
                                                  CancellationToken cancellationToken);

        Task CreateAsync(ResourceDto resourceDto, CancellationToken cancellationToken);

        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
