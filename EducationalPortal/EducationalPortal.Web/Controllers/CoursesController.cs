﻿using EducationalPortal.Application.Interfaces;
using EducationalPortal.Core.Entities;
using EducationalPortal.Web.Paging;
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

        [HttpGet]
        public async Task<IActionResult> Index(PageParameters pageParameters )
        {
            var courses = await this._coursesService
                                    .GetPageAsync(pageParameters.PageSize, pageParameters.PageNumber);
            var totalCount = await this._coursesService.GetCountAsync();
            var pagedCourses = new PagedList<Course>(courses, pageParameters, totalCount);

            return View(pagedCourses);
        }
    }
}
