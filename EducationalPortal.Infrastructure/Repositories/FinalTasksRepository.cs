using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities.FinalTasks;
using EducationalPortal.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace EducationalPortal.Infrastructure.Repositories
{
    public class FinalTasksRepository : IFinalTasksRepository
    {
        private readonly ApplicationContext _db;

        private readonly DbSet<FinalTask> _table;

        public FinalTasksRepository(ApplicationContext context)
        {
            this._db = context;
            this._table = this._db.FinalTasks;
        }

        public async Task AddAsync(FinalTask finalTask, CancellationToken cancellationToken)
        {
            this._db.Attach(finalTask);
            await this._table.AddAsync(finalTask, cancellationToken);
            await this.SaveAsync(cancellationToken);
        }

        public async Task DeleteAsync(FinalTask finalTask, CancellationToken cancellationToken)
        {
            this._table.Remove(finalTask);
            await this.SaveAsync(cancellationToken);
        }

        public Task<FinalTask?> GetFinalTaskAsync(int id, CancellationToken cancellationToken)
        {
            return this._table.FirstOrDefaultAsync(ft => ft.Id == id, cancellationToken);
        }

        public Task<FinalTask?> GetFinalTaskByCourseIdAsync(int courseId, CancellationToken cancellationToken)
        {
            return this._db.Courses
                .Where(c => c.Id == courseId)
                .Select(c => c.FinalTask)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<FinalTask?> GetFullFinalTaskAsync(int id, CancellationToken cancellationToken)
        {
            return this._table
                .Include(ft => ft.ReviewQuestions)
                .FirstOrDefaultAsync(ft => ft.Id == id, cancellationToken);
        }

        public async Task<PagedList<FinalTask>> GetPageAsync(PageParameters pageParameters, CancellationToken cancellationToken)
        {
            var finalTasks = await this._table.AsNoTracking()
                                              .Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                                              .Take(pageParameters.PageSize)
                                              .ToListAsync(cancellationToken);
            var totalCount = await this._table.CountAsync(cancellationToken);

            return new PagedList<FinalTask>(finalTasks, pageParameters, totalCount);
        }

        public Task<bool> ExistsAsync(string name, CancellationToken cancellationToken)
        {
            return this._table.AnyAsync(ft => ft.Name == name, cancellationToken);
        }

        public async Task<SubmittedFinalTask?> AddSubmittedFinalTaskAsync(SubmittedFinalTask finalTask, CancellationToken cancellationToken)
        {
            this._db.Attach(finalTask);
            await this._db.SubmittedFinalTasks.AddAsync(finalTask, cancellationToken);
            await this.SaveAsync(cancellationToken);

            return finalTask;
        }

        public async Task UpdateSubmittedFinalTaskAsync(SubmittedFinalTask finalTask, CancellationToken cancellationToken)
        {
            this._db.Attach(finalTask);
            this._db.Entry(finalTask).Property(ft => ft.Mark).IsModified = true;
            this._db.Entry(finalTask).Property(ft => ft.RevievedBy).IsModified = true;
            await this.SaveAsync(cancellationToken);
        }

        public Task<SubmittedFinalTask?> GetSubmittedFinalTaskAsync(int id, CancellationToken cancellationToken)
        {
            return this._db.SubmittedFinalTasks.FirstOrDefaultAsync(sft => sft.Id == id, cancellationToken);
        }

        public Task<SubmittedFinalTask?> GetSubmittedFinalTaskAsync(int finalTaskId, string reviewerId, CancellationToken cancellationToken)
        {
            return this._db.SubmittedFinalTasks
                .FirstOrDefaultAsync(sft => sft.FinalTaskId == finalTaskId 
                                         && sft.UserId == reviewerId, cancellationToken);
        }

        public async Task AddSubmittedReviewQuestionsAsync(IEnumerable<SubmittedReviewQuestion> questions, CancellationToken cancellationToken)
        {
            await this._db.SubmittedReviewQuestions.AddRangeAsync(questions, cancellationToken);
        }

        public async Task<IEnumerable<ReviewQuestion>> GetReviewQuestionsAsync(int finalTaskId, CancellationToken cancellationToken)
        {
            return await this._db.ReviewQuestions
                                 .Where(q => q.FinalTask.Id == finalTaskId)
                                 .ToListAsync(cancellationToken);
        }

        public Task<SubmittedFinalTask?> GetSubmittedFinalTaskForReviewAsync(int finalTaskId, CancellationToken cancellationToken)
        {
            return this._db.SubmittedFinalTasks
                .Where(s => s.FinalTaskId == finalTaskId
                       && s.SubmitDateUTC.Ticks + s.FinalTask.ReviewDeadlineTime.Ticks > DateTime.UtcNow.Ticks)
                .OrderBy(s => s.SubmitDateUTC)
                .FirstOrDefaultAsync(cancellationToken);
        }

        private async Task SaveAsync(CancellationToken cancellationToken)
        {
            await this._db.SaveChangesAsync(cancellationToken);
        }
    }
}
