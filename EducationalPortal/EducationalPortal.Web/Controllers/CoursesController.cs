using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Web.Mapping;
using EducationalPortal.Web.Paging;
using EducationalPortal.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EducationalPortal.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICoursesService _coursesService;

        private readonly IVideosService _videosService;

        private readonly IBooksService _booksService;

        private readonly IArticlesService _articlesService;

        private readonly IUsersRepository _usersRepository;

        private readonly IGenericRepository<MaterialsBase> _materialsRepository;

        private readonly Mapper _mapper = new();

        public CoursesController(ICoursesService coursesService, IVideosService videosService, 
                                 IBooksService booksService, IArticlesService articlesService,
                                 IUsersRepository usersRepository,
                                 IGenericRepository<MaterialsBase> materialsRepository)
        {
            this._coursesService = coursesService;
            this._videosService = videosService;
            this._booksService = booksService;
            this._articlesService = articlesService;
            this._usersRepository = usersRepository;
            this._materialsRepository = materialsRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(PageParameters pageParameters )
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
                var courseViewModel = this._mapper.Map(course);
                var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                courseViewModel.Materials = await this.MapMaterials(course.Materials, email);
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
            var learnCourse = await this.MapCourse(course, email);
            var userCourse = await this._usersRepository.GetUsersCoursesAsync(course.Id, email);
            learnCourse.Progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);

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

        public async Task<PartialViewResult> Learned(int materialId, int courseId)
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userCourse = await this._usersRepository.GetUsersCoursesAsync(courseId, email);
            userCourse.LearnedMaterialsCount++;
            await this._usersRepository.UpdateUsersCoursesAsync(userCourse);

            var user = await this._usersRepository.GetUserWithMaterialsAsync(email);
            user.Materials.Add(await this._materialsRepository.GetOneAsync(materialId));
            await this._usersRepository.UpdateAsync(user);

            var progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);
            return PartialView("_Progress", progress);
        }

        public async Task<PartialViewResult> Unlearned(int materialId, int courseId)
        {
            var email = User?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userCourse = await this._usersRepository.GetUsersCoursesAsync(courseId, email);
            userCourse.LearnedMaterialsCount--;
            await this._usersRepository.UpdateUsersCoursesAsync(userCourse);

            var user = await this._usersRepository.GetUserWithMaterialsAsync(email);
            user.Materials.Remove(user.Materials.FirstOrDefault(m => m.Id == materialId));
            await this._usersRepository.UpdateAsync(user);

            var progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);
            return PartialView("_Progress", progress);
        }

        private async Task<LearnCourseViewModel> MapCourse(Course course, string email)
        {
            var learnCourse = new LearnCourseViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Materials = await this.MapMaterials(course.Materials, email),
            };

            return learnCourse;
        }

        private async Task<List<MaterialsBaseViewModel>> MapMaterials(List<MaterialsBase> materials, 
                                                                      string email)
        {
            var materialsViewModel = new List<MaterialsBaseViewModel>();
            var user = await this._usersRepository.GetUserWithMaterialsAsync(email);
            foreach (var material in materials)
            {
                switch (material.GetType().Name)
                {
                    case "Video":
                        var video = (Video)material;
                        var videoViewModel = this._mapper.Map(video);
                        videoViewModel.IsLearned = user.Materials.Any(m => m.Id == material.Id);
                        materialsViewModel.Add(videoViewModel);
                        break;

                    case "Book":
                        var book = (Book)material;
                        var bookViewModel = this._mapper.Map(book);
                        bookViewModel.IsLearned = user.Materials.Any(m => m.Id == material.Id);
                        materialsViewModel.Add(bookViewModel);
                        break;

                    case "Article":
                        var article = (Article)material;
                        var articleViewModel = this._mapper.Map(article);
                        articleViewModel.IsLearned = user.Materials.Any(m => m.Id == material.Id);
                        materialsViewModel.Add(articleViewModel);
                        break;

                    default:
                        break;
                }
            }

            return materialsViewModel;
        }
    }
}
