using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Mapping;
using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.Course;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities.EducationalMaterials;

namespace EducationalPortal.Infrastructure.Services
{
    public class CoursesService : ICoursesService
    {
        private readonly ICoursesRepository _coursesRepository;

        private readonly IUsersRepository _usersRepository;

        private readonly IUsersCoursesRepository _usersCoursesRepository;
        
        private readonly IAccountService _accountService;

        private readonly IGenericRepository<MaterialsBase> _materialsRepository;

        private readonly Mapper _mapper = new();

        public CoursesService(ICoursesRepository coursesRepository, IUsersRepository usersRepository,
                              IUsersCoursesRepository usersCoursesRepository, IAccountService accountService,
                              IGenericRepository<MaterialsBase> materialsRepository)
        {
            this._coursesRepository = coursesRepository;
            this._usersRepository = usersRepository;
            this._usersCoursesRepository = usersCoursesRepository;
            this._accountService = accountService;
            this._materialsRepository = materialsRepository;
        }

        public async Task CreateAsync(CourseCreateDto courseDto, string authorEmail)
        {
            var course = this._mapper.Map(courseDto);
            var author = await this._usersRepository.GetUserAsync(authorEmail);
            course.Author = author;
            await this._coursesRepository.AddAsync(course);
        }

        public async Task UpdateAsync(int id, CourseCreateDto courseDto)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id);
            if (course == null)
            {
                throw new NotFoundException("Course");
            }

            this._mapper.Map(course, courseDto);
            await this._coursesRepository.UpdateAsync(course);
        }

        public async Task DeleteAsync(int id)
        {
            var course = await this._coursesRepository.GetCourseAsync(id);
            if (course == null)
            {
                throw new NotFoundException("Course");
            }
            await this._coursesRepository.DeleteAsync(course);
        }

        public async Task<CourseDto> GetCourseAsync(int id, string? email)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id);
            if (course == null)
            {
                throw new NotFoundException("Course");
            }

            var user = await this._usersRepository.GetUserWithMaterialsAsync(email);
            var dto = this._mapper.Map(course, user?.Materials);

            return dto;
        }

        public async Task<CourseCreateDto> GetCourseForEditAsync(int id)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id);
            if (course == null)
            {
                throw new NotFoundException("Course");
            }

            var courseDTO = this._mapper.Map(course);
            return courseDTO;
        }

        public async Task<CourseLearnDto> GetCourseLearnAsync(int id, string email)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id);
            if (course == null)
            {
                throw new NotFoundException("Course");
            }

            var user = await this._usersRepository.GetUserWithMaterialsAsync(email);
            var dto = this._mapper.MapLearnCourse(course, user?.Materials);
            var userCourse = await this._usersCoursesRepository.GetUsersCoursesAsync(course.Id, email);
            dto.Progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);

            return dto;
        }

        public async Task<PagedList<CourseShortDto>> GetPageAsync(PageParameters pageParameters)
        {
            var courses = await this._coursesRepository.GetPageAsync(pageParameters);
            var coursesDtos = this._mapper.Map(courses);
            return coursesDtos;
        }

        public async Task<int> MaterialLearnedAsync(int materialId, int courseId, string email)
        {
            var userCourse = await this._usersCoursesRepository.GetUsersCoursesAsync(courseId, email);
            if (userCourse == null)
            {
                throw new NotFoundException("User and Course");
            }

            userCourse.LearnedMaterialsCount++;
            await this._usersCoursesRepository.UpdateUsersCoursesAsync(userCourse);

            var user = await this._usersRepository.GetUserWithMaterialsAsync(email);
            user.Materials.Add(await this._materialsRepository.GetOneAsync(materialId));
            await this._usersRepository.UpdateAsync(user);

            var progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);
            if (progress == 100)
            {
                await this._accountService.AddAcquiredSkillsAsync(courseId, email);
            }

            return progress;
        }

        public async Task<int> MaterialUnearnedAsync(int materialId, int courseId, string email)
        {
            var userCourse = await this._usersCoursesRepository.GetUsersCoursesAsync(courseId, email);
            if (userCourse == null)
            {
                throw new NotFoundException("User and Course");
            }

            userCourse.LearnedMaterialsCount--;
            await this._usersCoursesRepository.UpdateUsersCoursesAsync(userCourse);

            var user = await this._usersRepository.GetUserWithMaterialsAsync(email);
            user.Materials.Remove(user.Materials.FirstOrDefault(m => m.Id == materialId));
            await this._usersRepository.UpdateAsync(user);

            var progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);

            return progress;
        }
    }
}
