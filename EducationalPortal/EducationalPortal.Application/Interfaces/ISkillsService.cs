using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces
{
    public interface ISkillsService
    {
        Task<PagedList<SkillDto>> GetPageAsync(PageParameters pageParameters);

        Task CreateAsync(SkillDto skillDto);

        Task DeleteAsync(int id);
    }
}
