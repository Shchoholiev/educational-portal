using EducationalPortal.Application.DTO;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Paging;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Web.Mapping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducationalPortal.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly IUsersService _usersService;

        private readonly ICloudStorageService _cloudStorageService;

        private readonly ICoursesRepository _coursesRepository;

        private readonly IGenericRepository<MaterialsBase> _materialsRepository;

        private readonly Mapper _mapper = new();

        public CoursesController(IUsersService usersService, ICloudStorageService cloudStorageService,
                                 ICoursesRepository coursesRepository,
                                 IGenericRepository<MaterialsBase> materialsRepository)
        {
            this._coursesRepository = coursesRepository;
            this._usersService = usersService;
            this._cloudStorageService = cloudStorageService;
            this._materialsRepository = materialsRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(PageParameters pageParameters)
        {
            var courses = await this._coursesRepository.GetPageAsync(pageParameters);
            return View(courses);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id);
            if (User.Identity.IsAuthenticated)
            {
                var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var user = await this._usersService.GetUserWithMaterialsAsync(email);
                var courseViewModel = this._mapper.Map(course, user.Materials);
                return View("DetailsAuthorized", courseViewModel);
            }
            else
            {
                return View(course);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Learn(int id)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id);
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await this._usersService.GetUserWithMaterialsAsync(email);
            var learnCourse = this._mapper.MapLearnCourse(course, user.Materials);
            var userCourse = await this._usersService.GetUsersCoursesAsync(course.Id, email);
            learnCourse.Progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);

            return View(learnCourse);
        }

        public async Task<PartialViewResult> Learned(int materialId, int courseId)
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
            return PartialView("_Progress", progress);
        }

        public async Task<PartialViewResult> Unlearned(int materialId, int courseId)
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userCourse = await this._usersService.GetUsersCoursesAsync(courseId, email);
            userCourse.LearnedMaterialsCount--;
            await this._usersService.UpdateUsersCoursesAsync(userCourse);

            var user = await this._usersService.GetUserWithMaterialsAsync(email);
            user.Materials.Remove(user.Materials.FirstOrDefault(m => m.Id == materialId));
            await this._usersService.UpdateUserAsync(user);

            var progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);
            return PartialView("_Progress", progress);
        }

        [HttpGet]
        [Authorize(Roles = "Creator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> Create(CourseDTO courseDTO)
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

                    return Json(new { success = true });
                }
            }

            return PartialView("_CreateCourse", courseDTO);
        }

        [HttpPost]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> GetThumbnail(IFormFile file)
        {
            var link = String.Empty;
            using (var stream = file.OpenReadStream())
            {
                link = await this._cloudStorageService.UploadAsync(stream, file.FileName, file.ContentType,
                                                                   "thumbnails");
            }

            return PartialView("_Thumbnail", link);
        }

        [HttpGet]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> Edit(int id)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id);
            var courseDTO = this._mapper.Map(course);
            return View(courseDTO);
        }

        [HttpPost]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> Edit(CourseDTO courseDTO)
        {
            if (ModelState.IsValid)
            {
                if (await this._coursesRepository.Exists(c => c.Name == courseDTO.Name && c.Id != courseDTO.Id))
                {
                    ModelState.AddModelError(string.Empty, "Course with this name already exists!");
                }
                else
                {
                    var mappedCourse = this._mapper.Map(courseDTO);
                    var course = await this._coursesRepository.GetCourseAsync(courseDTO.Id);

                    course.Name = mappedCourse.Name;
                    course.ShortDescription = mappedCourse.ShortDescription;
                    course.Description = mappedCourse.Description;
                    course.Price = mappedCourse.Price;
                    course.Thumbnail = mappedCourse.Thumbnail;
                    course.CoursesMaterials = mappedCourse.CoursesMaterials;
                    course.CoursesSkills = mappedCourse.CoursesSkills;

                    await this._coursesRepository.UpdateAsync(course);
                    return Json(new { success = true });
                }
            }

            return PartialView("_EditCourse", courseDTO);
        }

        [HttpGet]
        [Authorize(Roles = "Creator")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id);
            await this._coursesRepository.DeleteAsync(course);
            return RedirectToAction("Profile", "Account");
        }
    }
}
