using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.Course;

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
            var courses = await this._coursesService.GetPageAsync(pageParameters, cancellationToken);
            this.SetPagingMetadata(courses);
            return courses;
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
        public async Task<IActionResult> CreateAsync([FromBody] CourseCreateDto courseDto, 
                                                     CancellationToken cancellationToken)
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
