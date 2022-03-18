using EducationalPortal.Application.Interfaces;
using EducationalPortal.Core.Entities;
using EducationalPortal.Web.Mapping;
using EducationalPortal.Web.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducationalPortal.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICoursesService _coursesService;

        private readonly IVideosService _videosService;

        private readonly IBooksService _booksService;

        private readonly IArticlesService _articlesService;

        private readonly Mapper _mapper = new();

        public CoursesController(ICoursesService coursesService, IVideosService videosService, 
                                 IBooksService booksService, IArticlesService articlesService)
        {
            this._coursesService = coursesService;
            this._videosService = videosService;
            this._booksService = booksService;
            this._articlesService = articlesService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(PageParameters pageParameters )
        {
            var courses = await this._coursesService
                                    .GetPageAsync(pageParameters.PageSize, pageParameters.PageNumber);
            var totalCount = await this._coursesService.GetCountAsync();
            var pagedCourses = new PagedList<Course>(courses, pageParameters, totalCount);

            return View(pagedCourses);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var course = await this._coursesService.GetCourseAsync(id);
            return View(course);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Learn(int id)
        {
            var course = await this._coursesService.GetCourseAsync(id);
            var learnCourse = this._mapper.Map(course);
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            learnCourse.Progress = (await this._coursesService.GetUsersCoursesAsync(course.Id, email)).Progress;

            return View(learnCourse);
        }

        public async Task<PartialViewResult> Video(int id)
        {
            var video = await this._videosService.GetOneAsync(id);
            return PartialView("_Video", video);
        }

        public async Task<PartialViewResult> Book(int id)
        {
            var book = await this._booksService.GetOneAsync(id);
            return PartialView("_Book", book);
        }
    }
}
