using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.EducationalMaterials;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces.EducationalMaterials
{
    public interface IArticlesService
    {
        Task<PagedList<ArticleDto>> GetPageAsync(PageParameters pageParameters);

        Task Create(ArticleCreateDto articleDto);

        Task Update(ArticleCreateDto articleDto);

        Task Delete(int id);
    }
}
