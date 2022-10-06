using EducationalPortal.Application.Models.DTO.FinalTasks;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces
{
    public interface IFinalTasksService
    {
        Task CreateAsync(FinalTaskDto finalTaskDto, CancellationToken cancellationToken);

        Task<PagedList<FinalTaskDto>> GetPageAsync(PageParameters pageParameters, CancellationToken cancellationToken);

        Task DeleteAsync(int id, CancellationToken cancellationToken);

        Task SubmitAsync(FinalTaskDto finalTaskDto, CancellationToken cancellationToken);

        Task ReviewAsync(FinalTaskDto finalTaskDto, CancellationToken cancellation);
    }
}
