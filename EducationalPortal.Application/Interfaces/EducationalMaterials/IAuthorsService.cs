using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces.EducationalMaterials
{
    public interface IAuthorsService
    {
        Task<PagedList<AuthorDto>> GetPageAsync(PageParameters pageParameters, 
                                                CancellationToken cancellationToken);

        Task CreateAsync(AuthorDto authorDto, CancellationToken cancellationToken);

        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
