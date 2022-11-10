using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.Course;
using EducationalPortal.Core.Enums;
using EducationalPortal.Application.Models.LookupModels;

namespace EducationalPortal.API.Controllers
{
    [Authorize]
    public class CoursesController : ApiControllerBase
    {
        private readonly ICoursesService _coursesService;

        public CoursesController(ICoursesService coursesService)
        {
            this._coursesService = coursesService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<CourseShortDto>> GetCoursesAsync([FromQuery] PageParameters pageParameters, 
                                                                       CancellationToken cancellationToken)
        {
            var courses = await this._coursesService.GetPageAsync(pageParameters, UserId, cancellationToken);
            this.SetPagingMetadata(courses);
            return courses;
        }

        [HttpGet("filtered/{pageNumber}/{pageSize}/{orderBy}/{isAscending}/{filter?}")]
        [AllowAnonymous]
        public async Task<IEnumerable<CourseShortDto>> GetFilteredPageAsync(int pageNumber, int pageSize, 
            CoursesOrderBy orderBy, bool isAscending, CancellationToken cancellationToken, string filter = "")
        {
            var courses = await this._coursesService.GetFilteredPageAsync(new PageParameters(pageNumber, pageSize),
                filter, orderBy, isAscending, UserId, cancellationToken);
            this.SetPagingMetadata(courses);
            return courses;
        }

        [HttpPost("automated-search")]
        [AllowAnonymous]
        public async Task<IEnumerable<CourseShortDto>> GetCoursesByAutomatedSearchAsync(
            [FromBody] List<SkillLookupModel> skillLookups, CancellationToken cancellationToken)
        {
            return await this._coursesService.GetCoursesByAutomatedSearchAsync(skillLookups, UserId, cancellationToken);
        }

        [HttpPost("automated-search-based-on-time")]
        [AllowAnonymous]
        public async Task<IEnumerable<CourseShortDto>> GetCoursesByAutomatedSearchBasedOnTimeAsync(
            [FromBody] List<SkillLookupModel> skillLookups, CancellationToken cancellationToken)
        {
            return await this._coursesService.GetCoursesByAutomatedSearchBasedOnTimeAsync(skillLookups, 
                UserId ?? string.Empty, cancellationToken);
        }

        [HttpPost("pdf-for-automated-search")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPdfForAutomatedSearchAsync(
            [FromBody] List<SkillLookupModel> skillLookups, CancellationToken cancellationToken)
        {
            var bytes = await this._coursesService.GetPdfForAutomatedSearchAsync(skillLookups, 
                UserId ?? string.Empty, cancellationToken);
            var result = new FileContentResult(bytes, "application/pdf");
            Response.Headers.Add("Content-Disposition", $"inline; filename=Search Results.pdf");

            return result;
        }

        [HttpPost("pdf-for-automated-search-based-on-time")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPdfForAutomatedSearchBasedOnTimeAsync(
            [FromBody] List<SkillLookupModel> skillLookups, CancellationToken cancellationToken)
        {
            var bytes = await this._coursesService.GetPdfForAutomatedSearchBasedOnTimeAsync(skillLookups, 
                UserId ?? string.Empty, cancellationToken);
            var result = new FileContentResult(bytes, "application/pdf");
            Response.Headers.Add("Content-Disposition", $"inline; filename=Search Results.pdf");

            return result;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<CourseDto> GetCourseAsync(int id, CancellationToken cancellationToken)
        {
            return await this._coursesService.GetCourseAsync(id, UserId, cancellationToken);
        }

        [HttpGet("learn/{id}")]
        public async Task<CourseLearnDto> GetCourseToLearnAsync(int id, CancellationToken cancellationToken)
        {
            return await this._coursesService.GetCourseLearnAsync(id, UserId, cancellationToken);
        }

        [HttpPost]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> CreateAsync([FromBody] CourseCreateDto courseDto, CancellationToken cancellationToken)
        {
            var course = await this._coursesService.CreateAsync(courseDto, UserId, cancellationToken);
            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }

        [HttpGet("edit/{id}")]
        [Authorize(Roles = "Creator")]
        public async Task<CourseCreateDto> GetCourseForEdit(int id, CancellationToken cancellationToken)
        {
            return await this._coursesService.GetCourseForEditAsync(id, cancellationToken);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CourseCreateDto courseDto, 
                                                     CancellationToken cancellationToken)
        {
            await this._coursesService.UpdateAsync(id, courseDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await this._coursesService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPut("learned")]
        public async Task<int> LearnedAsync([FromQuery]int materialId, [FromQuery]int courseId, CancellationToken cancellationToken)
        {
            return await this._coursesService.MaterialLearnedAsync(materialId, courseId, UserId, cancellationToken);
        }

        [HttpPut("unlearned")]
        public async Task<int> UnlearnedAsync([FromQuery]int materialId, [FromQuery]int courseId, CancellationToken cancellationToken)
        {
            return await this._coursesService.MaterialUnearnedAsync(materialId, courseId, UserId, cancellationToken);
        }
    }
}
