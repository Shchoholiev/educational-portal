using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Mapping;
using EducationalPortal.Application.Models.DTO.FinalTasks;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.FinalTasks;
using Microsoft.Extensions.Logging;

namespace EducationalPortal.Infrastructure.Services
{
    public class FinalTasksService : IFinalTasksService
    {
        private readonly IFinalTasksRepository _finalTasksRepository;

        private readonly ILogger _logger;

        private readonly Mapper _mapper = new();

        public FinalTasksService(IFinalTasksRepository finalTasksRepository, ILogger<FinalTasksService> logger)
        {
            this._finalTasksRepository = finalTasksRepository;
            this._logger = logger;
        }

        public async Task<FinalTaskDto> GetFinalTaskAsync(int courseId, CancellationToken cancellationToken)
        {
            var finalTask = await this._finalTasksRepository.GetFinalTaskByCourseIdAsync(courseId, cancellationToken);
            if (finalTask == null)
            {
                throw new NotFoundException("FinalTask");
            }

            var dto = this._mapper.Map(finalTask);

            this._logger.LogInformation($"Returned FinalTask with Id: {finalTask.Id}.");

            return dto;
        }

        public async Task<SubmittedFinalTaskDto> GetSubmittedFinalTaskAsync(int finalTaskId, string userId, CancellationToken cancellationToken)
        {
            var submittedTask = await this._finalTasksRepository.GetSubmittedFinalTaskAsync(finalTaskId, userId, cancellationToken);
            if (submittedTask == null)
            {
                throw new NotFoundException("SubmittedFinalTask");
            }

            var dto = this._mapper.Map(submittedTask);
            dto.ReviewedTask = await this._finalTasksRepository.ReviewedOtherTaskAsync(finalTaskId, userId, cancellationToken);

            this._logger.LogInformation($"Returned SubmittedFinalTask with Id: {submittedTask.Id}.");

            return dto;
        }

        public async Task<FinalTaskForReview> GetFinalTaskForReviewAsync(int courseId, string userId, CancellationToken cancellationToken)
        {
            var finalTask = await this._finalTasksRepository.GetFinalTaskByCourseIdAsync(courseId, cancellationToken);
            if (finalTask == null)
            {
                throw new NotFoundException("FinalTask");
            }

            var submittedTask = await this._finalTasksRepository.GetSubmittedFinalTaskForReviewAsync(finalTask.Id, userId, cancellationToken);
            if (submittedTask == null)
            {
                throw new NotFoundException("SubmittedFinalTask");
            }

            var taskForReview = new FinalTaskForReview
            {
                ReviewQuestions = this._mapper.Map(finalTask.ReviewQuestions),
                SubmittedFinalTaskId = submittedTask.Id,
                FileLink = submittedTask.FileLink,
            };

            return taskForReview;
        }

        public async Task CreateAsync(FinalTaskDto finalTaskDto, CancellationToken cancellationToken)
        {
            var finalTask = this._mapper.Map(finalTaskDto);
            await this._finalTasksRepository.AddAsync(finalTask, cancellationToken);

            this._logger.LogInformation($"Created FinalTask with Id: {finalTask.Id}.");
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            if (await this._finalTasksRepository.IsUsedAsync(id, cancellationToken))
            {
                throw new DeleteEntityException("This article is used in other courses!");
            }

            var finalTask = await this._finalTasksRepository.GetFinalTaskAsync(id, cancellationToken);
            if (finalTask == null)
            {
                throw new NotFoundException("FinalTask");
            }
            await this._finalTasksRepository.DeleteAsync(finalTask, cancellationToken);

            this._logger.LogInformation($"Deleted FinalTask with Id: {finalTask.Id}.");
        }

        public async Task<PagedList<FinalTaskDto>> GetPageAsync(PageParameters pageParameters, CancellationToken cancellationToken)
        {
            var tasks = await this._finalTasksRepository.GetPageAsync(pageParameters, cancellationToken);
            var tasksDtos = this._mapper.Map(tasks);

            this._logger.LogInformation($"Returned FinalTasks page {tasks.PageNumber} from database.");

            return tasksDtos;
        }

        public async Task SubmitAsync(SubmittedFinalTaskDto submittedTaskDto, CancellationToken cancellationToken)
        {
            var submittedTask = this._mapper.Map(submittedTaskDto);
            submittedTask.SubmitDateUTC = DateTime.UtcNow;
            var dbTask = await this._finalTasksRepository.AddSubmittedFinalTaskAsync(submittedTask, cancellationToken);

            this._logger.LogInformation($"Created SudmittedFinalTask with Id: {dbTask.Id}.");
        }

        public async Task ReviewAsync(ReviewedFinalTask reviewedFinalTask, CancellationToken cancellationToken)
        {
            var submittedFinalTask = await this._finalTasksRepository.GetSubmittedFinalTaskAsync(
                reviewedFinalTask.SubmittedFinalTaskId, cancellationToken);
            if (submittedFinalTask == null)
            {
                throw new NotFoundException("SubmittedFinalTask");
            }

            var finalTask = await this._finalTasksRepository.GetFinalTaskAsync(submittedFinalTask.FinalTaskId, cancellationToken);
            if (finalTask == null)
            {
                throw new NotFoundException("FinalTask");
            }

            await this.CheckDeadlineAsync(finalTask, reviewedFinalTask.ReviewerId, cancellationToken);

            var reviewQuestions = await this._finalTasksRepository.GetReviewQuestionsAsync(
                submittedFinalTask.FinalTaskId, cancellationToken);

            var submittedReviewQuestions = this._mapper.Map(reviewedFinalTask.SubmittedReviewQuestions, reviewedFinalTask.SubmittedFinalTaskId);
            await this._finalTasksRepository.AddSubmittedReviewQuestionsAsync(submittedReviewQuestions, cancellationToken);

            submittedFinalTask.Mark = this.GetFinalTaskMark(reviewedFinalTask.SubmittedReviewQuestions, reviewQuestions);
            submittedFinalTask.ReviewedById = reviewedFinalTask.ReviewerId;

            await this._finalTasksRepository.UpdateSubmittedFinalTaskAsync(submittedFinalTask, cancellationToken);
        }

        private async Task CheckDeadlineAsync(FinalTask finalTask, string userId, CancellationToken cancellationToken)
        {
            var reviewerFinalTask = await this._finalTasksRepository.GetSubmittedFinalTaskAsync(
                finalTask.Id, userId, cancellationToken);
            if (reviewerFinalTask == null)
            {
                throw new InvalidOperationException("Reviewer did not submit his task!");
            }

            var deadlineTime = new TimeSpan(finalTask.ReviewDeadlineTime.Ticks);
            if (reviewerFinalTask.SubmitDateUTC.Add(deadlineTime) < DateTime.UtcNow)
            {
                throw new InvalidOperationException("Reviewer reviewed the task after the deadline!");
            }
        }

        private int GetFinalTaskMark(IEnumerable<SubmittedReviewQuestionDto> submittedReviewQuestionsDtos, 
                                     IEnumerable<ReviewQuestion> reviewQuestions)
        {
            var total = reviewQuestions.Sum(rq => rq.MaxMark);
            var scored = submittedReviewQuestionsDtos.Sum(srq => srq.Mark);
            return scored * 100 / total;
        }
    }
}
