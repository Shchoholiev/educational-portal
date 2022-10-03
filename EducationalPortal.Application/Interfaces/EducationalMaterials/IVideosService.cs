using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.EducationalMaterials;
using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces.EducationalMaterials
{
    public interface IVideosService
    {
        Task<PagedList<VideoDto>> GetPageAsync(PageParameters pageParameters, 
                                               CancellationToken cancellationToken);

        Task CreateAsync(VideoCreateDto videoDto, CancellationToken cancellationToken);

        Task DeleteAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<QualityDto>> GetQualitiesAsync(CancellationToken cancellationToken);
    }
}
