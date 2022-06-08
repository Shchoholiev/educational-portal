using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.Course;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Infrastructure.Services;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EducationalPortal.UnitTests.ServicesTests
{
    public class CoursesServiceTests
    {
        private readonly CoursesService _coursesService;

        private readonly ICoursesRepository _coursesRepository;

        private readonly IUsersRepository _usersRepository;

        private readonly IUsersCoursesRepository _usersCoursesRepository;

        private readonly IGenericRepository<MaterialsBase> _materialsRepository;

        private readonly ILogger<CoursesService> _logger;

        public CoursesServiceTests()
        {
            this._coursesRepository = A.Fake<ICoursesRepository>();
            this._usersRepository = A.Fake<IUsersRepository>();
            this._usersCoursesRepository = A.Fake<IUsersCoursesRepository>();
            this._materialsRepository = A.Fake<IGenericRepository<MaterialsBase>>();
            this._logger = A.Fake<ILogger<CoursesService>>();

            this._coursesService = new CoursesService(this._coursesRepository, this._usersRepository,
                this._usersCoursesRepository, this._materialsRepository, this._logger);
        }

        [Fact]
        public async Task GetCourseAsync_NotAuthorized_ReturnsCourseDto()
        {
            var courseDtoDummy = A.Dummy<CourseDto>();
            var courseDummy = A.Dummy<Course>();
            courseDummy.CoursesMaterials = new List<CoursesMaterials>();

            A.CallTo(() => this._coursesRepository.GetFullCourseAsync(1))
             .Returns(Task.FromResult(courseDummy));

            var result = await this._coursesService.GetCourseAsync(1, null);

            Assert.IsType<CourseDto>(result);
        }

        [Fact]
        public async Task GetCourseAsync_NotAuthorized_ReturnsNotFoundException()
        {
            var courseDtoDummy = A.Dummy<CourseDto>();
            Course courseDummy = null;

            A.CallTo(() => this._coursesRepository.GetFullCourseAsync(1))
             .Returns(Task.FromResult(courseDummy));

            Func<Task> act = () => this._coursesService.GetCourseAsync(1, null);

            await Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task GetCourseLearnAsync_ReturnsCourseLearnDto()
        {
            var courseDtoDummy = A.Dummy<CourseDto>();
            var courseDummy = A.Dummy<Course>();
            courseDummy.Id = 1;
            courseDummy.CoursesMaterials = new List<CoursesMaterials>();
            var userCourseDummy = new UsersCourses
            {
                CourseId = 1,
                LearnedMaterialsCount = 1,
                MaterialsCount = 3
            };

            A.CallTo(() => this._usersCoursesRepository.GetUsersCoursesAsync(1, "email"))
             .Returns(Task.FromResult(userCourseDummy));
            A.CallTo(() => this._coursesRepository.GetFullCourseAsync(1))
             .Returns(Task.FromResult(courseDummy));

            var result = await this._coursesService.GetCourseLearnAsync(1, "email");

            Assert.IsType<CourseLearnDto>(result);
        }

        [Fact]
        public async Task GetCourseLearnAsync_ReturnsNotFoundException()
        {
            var courseDtoDummy = A.Dummy<CourseDto>();
            Course courseDummy = null;

            A.CallTo(() => this._coursesRepository.GetFullCourseAsync(1))
             .Returns(Task.FromResult(courseDummy));

            Func<Task> act = () => this._coursesService.GetCourseLearnAsync(1, "");

            await Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task GetPageAsync_ReturnsPagedListWithDtos()
        {
            var pageParameters = new PageParameters();
            var coursesDummy = A.CollectionOfDummy<Course>(5).AsEnumerable();
            var pagedList = new PagedList<Course>(coursesDummy, pageParameters, 5);
            A.CallTo(() => this._coursesRepository.GetPageAsync(pageParameters))
             .Returns(Task.FromResult(pagedList));

            var result = await this._coursesService.GetPageAsync(pageParameters);

            Assert.Equal(5, result.Count());
            Assert.IsType<PagedList<CourseShortDto>>(result);
        }


    }
}
