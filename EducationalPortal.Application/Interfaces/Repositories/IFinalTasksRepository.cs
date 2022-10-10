using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities.FinalTasks;

namespace EducationalPortal.Application.Interfaces.Repositories
{
    public interface IFinalTasksRepository
    {
        Task AddAsync(FinalTask finalTask, CancellationToken cancellationToken);

        Task DeleteAsync(FinalTask finalTask, CancellationToken cancellationToken);

        Task<FinalTask?> GetFinalTaskAsync(int id, CancellationToken cancellationToken);

        Task<FinalTask?> GetFinalTaskByCourseIdAsync(int courseId, CancellationToken cancellationToken);

        Task<FinalTask?> GetFullFinalTaskAsync(int id, CancellationToken cancellationToken);

        Task<PagedList<FinalTask>> GetPageAsync(PageParameters pageParameters, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(string name, CancellationToken cancellationToken);

        Task<SubmittedFinalTask?> AddSubmittedFinalTaskAsync(SubmittedFinalTask finalTask, CancellationToken cancellationToken);

        Task UpdateSubmittedFinalTaskAsync(SubmittedFinalTask finalTask, CancellationToken cancellationToken);

        Task<SubmittedFinalTask?> GetSubmittedFinalTaskAsync(int id, CancellationToken cancellationToken);
        
        Task<SubmittedFinalTask?> GetSubmittedFinalTaskAsync(int finalTaskId, string reviewerId, CancellationToken cancellationToken);

        Task AddSubmittedReviewQuestionsAsync(IEnumerable<SubmittedReviewQuestion> questions, CancellationToken cancellationToken);

        Task<IEnumerable<ReviewQuestion>> GetReviewQuestionsAsync(int finalTaskId, CancellationToken cancellationToken);
        
        Task<SubmittedFinalTask?> GetSubmittedFinalTaskForReviewAsync(int finalTaskId, CancellationToken cancellationToken);
    }
}
