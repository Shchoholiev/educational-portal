using EducationalPortal.Application.DTO;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Web.Mapping;
using EducationalPortal.Web.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducationalPortal.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICoursesService _coursesService;

        private readonly IUsersService _usersService;

        private readonly ICloudStorageService _cloudStorageService;

        private readonly IGenericRepository<Video> _videosRepository;

        private readonly IGenericRepository<Book> _booksRepository;

        private readonly IGenericRepository<MaterialsBase> _materialsRepository;

        private readonly Mapper _mapper = new();

        public CoursesController(ICoursesService coursesService, IUsersService usersService,
                                 ICloudStorageService cloudStorageService,
                                 IGenericRepository<MaterialsBase> materialsRepository,
                                 IGenericRepository<Video> videosRepository,
                                 IGenericRepository<Book> booksRepository)
        {
            this._coursesService = coursesService;
            this._usersService = usersService;
            this._cloudStorageService = cloudStorageService;
            this._materialsRepository = materialsRepository;
            this._videosRepository = videosRepository;
            this._booksRepository = booksRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(PageParameters pageParameters)
        {
            var courses = await this._coursesService
                                    .GetPageAsync(pageParameters.PageSize, pageParameters.PageNumber);
            var totalCount = await this._coursesService.GetCountAsync();
            var pagedCourses = new PagedList<Course>(courses, pageParameters, totalCount);

            return View(pagedCourses);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var course = await this._coursesService.GetCourseAsync(id);
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
            var course = await this._coursesService.GetCourseAsync(id);
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await this._usersService.GetUserWithMaterialsAsync(email);
            var learnCourse = this._mapper.MapLearnCourse(course, user.Materials);
            var userCourse = await this._usersService.GetUsersCoursesAsync(course.Id, email);
            learnCourse.Progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);

            return View(learnCourse);
        }

        public async Task<PartialViewResult> Video(int id)
        {
            var video = await this._videosRepository.GetOneAsync(id, v => v.Quality);
            return PartialView("_Video", video);
        }

        public async Task<PartialViewResult> Book(int id)
        {
            var book = await this._booksRepository.GetOneAsync(id, b => b.Extension, b => b.Authors);
            return PartialView("_Book", book);
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
            if ((await this._coursesService.GetPageAsync(1, 1, c => c.Name == courseDTO.Name)).Count() > 0)
            {
                ModelState.AddModelError(string.Empty, "Course with this name already exists!");
                return View();
            }
            else
            {
                var course = this._mapper.Map(courseDTO);
                var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                course.Author = await _usersService.GetUserAsync(email);

                using (var stream = courseDTO.Thumbnail.OpenReadStream())
                {
                    course.Thumbnail = await this._cloudStorageService.UploadAsync(stream, courseDTO.Thumbnail.FileName,
                                                                    courseDTO.Thumbnail.ContentType, "thumbnails");
                }

                course.CoursesMaterials = new List<CoursesMaterials>();
                for (int i = 0; i < courseDTO.Materials.Count; i++)
                {
                    var courseMaterial = new CoursesMaterials
                    {
                        Material = courseDTO.Materials[i],
                        Index = i + 1,
                    };
                    course.CoursesMaterials.Add(courseMaterial);
                }
                await this._coursesService.AddCourseAsync(course);

                return Json(new { success = true });
            }
        }
    }
}
