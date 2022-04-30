using EducationalPortal.Application.DTO;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Paging;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.API.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using EducationalPortal.Core.Entities;
using Newtonsoft.Json;
using EducationalPortal.API.ViewModels;

namespace EducationalPortal.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/courses")]
    public class CoursesController : Controller
    {
        private readonly IUsersService _usersService;

        private readonly ICoursesRepository _coursesRepository;

        private readonly IGenericRepository<MaterialsBase> _materialsRepository;

        private readonly Mapper _mapper = new();

        public CoursesController(IUsersService usersService, ICloudStorageService cloudStorageService,
                                 ICoursesRepository coursesRepository,
                                 IGenericRepository<MaterialsBase> materialsRepository)
        {
            this._coursesRepository = coursesRepository;
            this._usersService = usersService;
            this._materialsRepository = materialsRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses([FromQuery]PageParameters pageParameters)
        {
            var courses = await this._coursesRepository.GetPageAsync(pageParameters);
            var metadata = new
            {
                courses.TotalItems,
                courses.PageSize,
                courses.PageNumber,
                courses.TotalPages,
                courses.HasNextPage,
                courses.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return courses;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<CourseViewModel>> GetCourse(int id)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            if (User.Identity.IsAuthenticated)
            {
                var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var user = await this._usersService.GetUserWithMaterialsAsync(email);
                var courseViewModel = this._mapper.Map(course, user?.Materials ?? new List<MaterialsBase>());
                return courseViewModel;
            }
            else
            {
                var courseViewModel = this._mapper.Map(course, new List<MaterialsBase>());
                return courseViewModel;
            }
        }

        [HttpGet("learn/{id}")]
        public async Task<ActionResult<LearnCourseViewModel>> GetCourseToLearn(int id)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id);
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await this._usersService.GetUserWithMaterialsAsync(email);
            var learnCourse = this._mapper.MapLearnCourse(course, user.Materials);
            var userCourse = await this._usersService.GetUsersCoursesAsync(course.Id, email);
            learnCourse.Progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);

            return learnCourse;
        }

        [HttpPost]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> Create([FromBody] CourseDTO courseDTO)
        {
            if (ModelState.IsValid)
            {
                if (await this._coursesRepository.Exists(c => c.Name == courseDTO.Name))
                {
                    ModelState.AddModelError(string.Empty, "Course with this name already exists!");
                }
                else
                {
                    var course = this._mapper.Map(courseDTO);
                    var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    course.Author = await _usersService.GetUserAsync(email);
                    await this._coursesRepository.AddAsync(course);
                    var createdCourse = this._mapper.Map(course, new List<MaterialsBase>());

                    return CreatedAtAction("GetCourse", new { id = course.Id }, createdCourse);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpGet("edit/{id}")]
        [Authorize(Roles = "Creator")]
        public async Task<ActionResult<CourseDTO>> Edit(int id)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id);
            var courseDTO = this._mapper.Map(course);
            return courseDTO;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> Edit(int id, [FromBody] CourseDTO courseDTO)
        {
            if (ModelState.IsValid)
            {
                if (await this._coursesRepository.Exists(c => c.Name == courseDTO.Name && c.Id != id))
                {
                    ModelState.AddModelError(string.Empty, "Course with this name already exists!");
                }
                else
                {

                    var course = await this._coursesRepository.GetFullCourseAsync(id);
                    this._mapper.Map(course, courseDTO);
                    await this._coursesRepository.UpdateAsync(course);
                    return NoContent();
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            await this._coursesRepository.DeleteAsync(course);

            return NoContent();
        }

        [HttpPut("learned")]
        public async Task<ActionResult<int>> Learned([FromQuery]int materialId, [FromQuery]int courseId)
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userCourse = await this._usersService.GetUsersCoursesAsync(courseId, email);
            userCourse.LearnedMaterialsCount++;
            await this._usersService.UpdateUsersCoursesAsync(userCourse);

            var user = await this._usersService.GetUserWithMaterialsAsync(email);
            user.Materials.Add(await this._materialsRepository.GetOneAsync(materialId));
            await this._usersService.UpdateUserAsync(user);

            var progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);
            if (progress == 100)
            {
                await this._usersService.AddAcquiredSkills(courseId, email);
            }

            return progress;
        }

        [HttpPut("unlearned")]
        public async Task<ActionResult<int>> Unlearned([FromQuery]int materialId, [FromQuery]int courseId)
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userCourse = await this._usersService.GetUsersCoursesAsync(courseId, email);
            userCourse.LearnedMaterialsCount--;
            await this._usersService.UpdateUsersCoursesAsync(userCourse);

            var user = await this._usersService.GetUserWithMaterialsAsync(email);
            user.Materials.Remove(user.Materials.FirstOrDefault(m => m.Id == materialId));
            await this._usersService.UpdateUserAsync(user);

            var progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);

            return progress;
        }
    }
}
