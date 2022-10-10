using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Models.DTO.FinalTasks;
using EducationalPortal.Application.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPortal.API.Controllers
{
    [Authorize]
    public class FinalTasksController : ApiControllerBase
    {
        private readonly IFinalTasksService _finalTasksService;

        public FinalTasksController(IFinalTasksService finalTasksService)
        {
            this._finalTasksService = finalTasksService;
        }

        [HttpGet("{courseId}")]
        public async Task<FinalTaskDto> GetFinalTaskAsync(int courseId, CancellationToken cancellationToken)
        {
            return await this._finalTasksService.GetFinalTaskAsync(courseId, cancellationToken);
        }

        [HttpGet("for-review/{courseId}")]
        public async Task<FinalTaskForReview> GetFinalTaskForReviewAsync(int courseId, CancellationToken cancellationToken)
        {
            return await this._finalTasksService.GetFinalTaskForReviewAsync(courseId, cancellationToken);
        }

        [HttpPost("submit")]
        public async Task SubmitAsync([FromBody] SubmittedFinalTaskDto submittedTaskDto, CancellationToken cancellationToken)
        {
            await this._finalTasksService.SubmitAsync(submittedTaskDto, cancellationToken);
        }

        [HttpPost("review")]
        public async Task ReviewAsync([FromBody] ReviewedFinalTask reviewedFinalTask, CancellationToken cancellationToken)
        {
            await this._finalTasksService.ReviewAsync(reviewedFinalTask, cancellationToken);
        }

        [HttpGet]
        [Authorize(Roles = "Creator")]
        public async Task<IEnumerable<FinalTaskDto>> GetFinalTasksAsync([FromQuery] PageParameters pageParameters,
                                                                       CancellationToken cancellationToken)
        {
            var finalTasks = await this._finalTasksService.GetPageAsync(pageParameters, cancellationToken);
            this.SetPagingMetadata(finalTasks);
            return finalTasks;
        }

        [HttpPost]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> CreateAsync([FromBody] FinalTaskDto finalTaskDto, CancellationToken cancellationToken)
        {
            await this._finalTasksService.CreateAsync(finalTaskDto, cancellationToken);
            return StatusCode(201);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await this._finalTasksService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
