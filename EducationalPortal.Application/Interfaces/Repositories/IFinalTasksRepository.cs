using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities.FinalTasks;

namespace EducationalPortal.Application.Interfaces.Repositories
{
    public interface IFinalTasksRepository
    {
        Task AddAsync(FinalTask finalTask, CancellationToken cancellationToken);

        Task DeleteAsync(FinalTask finalTask, CancellationToken cancellationToken);

        Task<FinalTask?> GetFinalTaskAsync(int id, CancellationToken cancellationToken);

        Task<PagedList<FinalTask>> GetPageAsync(PageParameters pageParameters, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(string name, CancellationToken cancellationToken);

        Task AddSudmittedFinalTaskAsync(SubmittedFinalTask finalTask, CancellationToken cancellationToken);

        Task AddSudmittedReviewQuestionsAsync(IEnumerable<SubmittedReviewQuestion> questions, CancellationToken cancellationToken);

        Task UpdateSudmittedFinalTaskAsync(SubmittedFinalTask finalTask, CancellationToken cancellationToken);
    }
}
