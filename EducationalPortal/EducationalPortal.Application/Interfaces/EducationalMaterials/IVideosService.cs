using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.EducationalMaterials;
using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces.EducationalMaterials
{
    public interface IVideosService
    {
        Task<PagedList<VideoDto>> GetPageAsync(PageParameters pageParameters);

        Task CreateAsync(VideoCreateDto videoDto);

        Task DeleteAsync(int id);

        Task<IEnumerable<QualityDto>> GetQualitiesAsync();
    }
}
