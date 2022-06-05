using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces
{
    public interface ISkillsService
    {
        Task<PagedList<SkillDto>> GetPageAsync(PageParameters pageParameters);

        Task Create(SkillDto skillDto);

        Task Delete(int id);
    }
}
