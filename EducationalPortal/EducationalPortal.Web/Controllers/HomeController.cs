using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;
using EducationalPortal.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EducationalPortal.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICoursesRepository _courseRepository;

        public HomeController(ICoursesRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public IActionResult Index()
        {
            //var course = await _courseRepository.GetCourseAsync(1);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}