using EducationalPortal.Application.Models.DTO.FinalTasks;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Interfaces
{
    public interface IFinalTasksService
    {
        Task<FinalTaskDto> GetFinalTaskAsync(int courseId, CancellationToken cancellationToken);

        Task CreateAsync(FinalTaskDto finalTaskDto, CancellationToken cancellationToken);

        Task<PagedList<FinalTaskDto>> GetPageAsync(PageParameters pageParameters, CancellationToken cancellationToken);

        Task DeleteAsync(int id, CancellationToken cancellationToken);

        Task SubmitAsync(SubmittedFinalTaskDto submittedTaskDto, CancellationToken cancellationToken);

        Task ReviewAsync(ReviewedFinalTask reviewedFinalTask, CancellationToken cancellation);
    }
}
