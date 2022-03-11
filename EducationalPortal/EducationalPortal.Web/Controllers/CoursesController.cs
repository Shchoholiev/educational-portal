using EducationalPortal.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPortal.Web.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ICoursesService _coursesService;

        public CoursesController(ICoursesService coursesService)
        {
            this._coursesService = coursesService;
        }

        public IActionResult Index()
        {
            var courses = this._coursesService.GetPage();
            return View();
        }
    }
}
