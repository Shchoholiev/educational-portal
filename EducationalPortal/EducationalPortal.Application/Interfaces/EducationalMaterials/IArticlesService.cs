using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.EducationalMaterials;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces.EducationalMaterials
{
    public interface IArticlesService
    {
        Task<PagedList<ArticleDto>> GetPageAsync(PageParameters pageParameters);

        Task CreateAsync(ArticleCreateDto articleDto);

        Task DeleteAsync(int id);
    }
}
