using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.Application.Interfaces
{
    public interface IVideosService : IMaterialsService<Video>
    {
        Task<IEnumerable<Quality>> GetQualitiesAsync();
    }
}
