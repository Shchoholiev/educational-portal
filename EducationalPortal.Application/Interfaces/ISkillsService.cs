using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces
{
    public interface ISkillsService
    {
        Task<PagedList<SkillDto>> GetPageAsync(PageParameters pageParameters, 
                                               CancellationToken cancellationToken);

        Task CreateAsync(SkillDto skillDto, CancellationToken cancellationToken);

        Task DeleteAsync(int id, CancellationToken cancellationToken);
    }
}
