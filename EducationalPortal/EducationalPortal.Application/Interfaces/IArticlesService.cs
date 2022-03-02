using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.Application.Interfaces
{
    public interface IArticlesService : IMaterialsService<Article>
    {
        Task<IEnumerable<Resource>> GetResources();
    }
}
