using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces.EducationalMaterials
{
    public interface IAuthorsService
    {
        Task<PagedList<AuthorDto>> GetPageAsync(PageParameters pageParameters);

        Task Create(AuthorDto authorDto);

        Task Delete(int id);
    }
}
