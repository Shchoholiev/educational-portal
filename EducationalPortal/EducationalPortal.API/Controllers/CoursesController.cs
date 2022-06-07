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
        public async Task<ActionResult<IEnumerable<CourseShortDto>>> GetCourses([FromQuery] PageParameters pageParameters)
        {
            var courses = await this._coursesService.GetPageAsync(pageParameters);
            this.SetPagingMetadata(courses);
            return courses;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            return await this._coursesService.GetCourseAsync(id, Email);
        }

        [HttpGet("learn/{id}")]
        public async Task<ActionResult<CourseLearnDto>> GetCourseToLearn(int id)
        {
            return await this._coursesService.GetCourseLearnAsync(id, Email);
        }

        [HttpPost]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> Create([FromBody] CourseCreateDto courseDTO)
        {
            var course = await this._coursesService.CreateAsync(courseDTO, Email);
            return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        }

        [HttpGet("edit/{id}")]
        [Authorize(Roles = "Creator")]
        public async Task<ActionResult<CourseCreateDto>> GetCourseForEdit(int id)
        {
            return await this._coursesService.GetCourseForEditAsync(id);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> Update(int id, [FromBody] CourseCreateDto courseDTO)
        {
            await this._coursesService.UpdateAsync(id, courseDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> Delete(int id)
        {
            await this._coursesService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("learned")]
        public async Task<ActionResult<int>> Learned([FromQuery]int materialId, [FromQuery]int courseId)
        {
            return await this._coursesService.MaterialLearnedAsync(materialId, courseId, Email);
        }

        [HttpPut("unlearned")]
        public async Task<ActionResult<int>> Unlearned([FromQuery]int materialId, [FromQuery]int courseId)
        {
            return await this._coursesService.MaterialUnearnedAsync(materialId, courseId, Email);
        }
    }
}
