using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces.EducationalMaterials
{
    public interface IAuthorsService
    {
        Task<PagedList<AuthorDto>> GetPageAsync(PageParameters pageParameters);

        Task CreateAsync(AuthorDto authorDto);

        Task DeleteAsync(int id);
    }
}
