using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.Application.Interfaces
{
    public interface IBooksService : IMaterialsService<Book>
    {
        Task<IEnumerable<Author>> GetAuthors();
    }
}
