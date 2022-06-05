using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.EducationalMaterials;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces
{
    public interface IVideosService
    {
        Task<PagedList<VideoDto>> GetPageAsync(PageParameters pageParameters);

        Task Create(VideoCreateDto videoDto);

        Task Update(VideoCreateDto videoDto);

        Task Delete(int id);
    }
}
