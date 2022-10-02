using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Mapping;
using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.Course;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using Microsoft.Extensions.Logging;

namespace EducationalPortal.Infrastructure.Services
{
    public class CoursesService : ICoursesService
    {
        private readonly ICoursesRepository _coursesRepository;

        private readonly IUsersRepository _usersRepository;

        private readonly IUsersCoursesRepository _usersCoursesRepository;

        private readonly IGenericRepository<MaterialsBase> _materialsRepository;

        private readonly ILogger _logger;

        private readonly Mapper _mapper = new();

        public CoursesService(ICoursesRepository coursesRepository, IUsersRepository usersRepository,
                              IUsersCoursesRepository usersCoursesRepository,
                              IGenericRepository<MaterialsBase> materialsRepository,
                              ILogger<CoursesService> logger)
        {
            this._coursesRepository = coursesRepository;
            this._usersRepository = usersRepository;
            this._usersCoursesRepository = usersCoursesRepository;
            this._materialsRepository = materialsRepository;
            this._logger = logger;
        }

        public async Task<Course> CreateAsync(CourseCreateDto courseDto, string authorEmail, 
                                              CancellationToken cancellationToken)
        {
            var course = this._mapper.Map(courseDto);
            var author = await this._usersRepository.GetUserAsync(authorEmail, cancellationToken);
            course.Author = author;
            await this._coursesRepository.AddAsync(course, cancellationToken);

            this._logger.LogInformation($"Created course with id: {course.Id}.");

            return course;
        }

        public async Task UpdateAsync(int id, CourseCreateDto courseDto, CancellationToken cancellationToken)
        {
            var course = await this._coursesRepository.GetCourseAsync(id, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException("Course");
            }

            this._mapper.Map(course, courseDto);
            await this._coursesRepository.UpdateAsync(course, cancellationToken);

            this._logger.LogInformation($"Updated course with id: {course.Id}.");
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var course = await this._coursesRepository.GetCourseAsync(id, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException("Course");
            }
            await this._coursesRepository.DeleteAsync(course, cancellationToken);

            this._logger.LogInformation($"Deleted course with id: {course.Id}.");
        }

        public async Task<CourseDto> GetCourseAsync(int id, string? userId, CancellationToken cancellationToken)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id, userId ?? string.Empty, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException("Course");
            }

            var dto = this._mapper.Map(course);

            this._logger.LogInformation($"Returned course with id: {course.Id}.");

            return dto;
        }

        public async Task<CourseCreateDto> GetCourseForEditAsync(int id, CancellationToken cancellationToken)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id, string.Empty, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException("Course");
            }

            var courseDTO = this._mapper.MapForEdit(course);

            this._logger.LogInformation($"Returned course for edit with id: {course.Id}.");

            return courseDTO;
        }

        public async Task<CourseLearnDto> GetCourseLearnAsync(int id, string userId, CancellationToken cancellationToken)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id, userId, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException("Course");
            }

            var dto = this._mapper.MapLearnCourse(course);
            var userCourse = await this._usersCoursesRepository.GetUsersCoursesAsync(course.Id, userId, cancellationToken);
            dto.Progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);

            this._logger.LogInformation($"Returned course learn with id: {course.Id}.");

            return new CourseLearnDto();
        }

        public async Task<PagedList<CourseShortDto>> GetPageAsync(PageParameters pageParameters, 
                                                                  CancellationToken cancellationToken)
        {
            var courses = await this._coursesRepository.GetPageAsync(pageParameters, cancellationToken);
            var coursesDtos = this._mapper.Map(courses);

            this._logger.LogInformation($"Returned courses page {courses.PageNumber} from database.");

            return coursesDtos;
        }

        public async Task<int> MaterialLearnedAsync(int materialId, int courseId, string email, 
                                                    CancellationToken cancellationToken)
        {
            var userCourse = await this._usersCoursesRepository.GetUsersCoursesAsync(courseId, email, cancellationToken);
            if (userCourse == null)
            {
                throw new NotFoundException("User and Course");
            }

            userCourse.LearnedMaterialsCount++;
            await this._usersCoursesRepository.UpdateUsersCoursesAsync(userCourse, cancellationToken);

            var user = await this._usersRepository.GetUserWithMaterialsAsync(email, cancellationToken);
            user.Materials.Add(await this._materialsRepository.GetOneAsync(materialId, cancellationToken));
            await this._usersRepository.UpdateAsync(user, cancellationToken);

            this._logger.LogInformation($"Material with id: {materialId} added to user with email: {email}." +
                                        $" Material learned.");

            var progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);
            if (progress == 100)
            {
                await this._usersRepository.AddAcquiredSkillsAsync(courseId, email, cancellationToken);
                this._logger.LogInformation($"Added skills of course with id: {courseId} " +
                                            $"to user with email: {email}. Course learned.");
            }

            return progress;
        }

        public async Task<int> MaterialUnearnedAsync(int materialId, int courseId, string email, 
                                                     CancellationToken cancellationToken)
        {
            var userCourse = await this._usersCoursesRepository.GetUsersCoursesAsync(courseId, email, cancellationToken);
            if (userCourse == null)
            {
                throw new NotFoundException("User and Course");
            }

            userCourse.LearnedMaterialsCount--;
            await this._usersCoursesRepository.UpdateUsersCoursesAsync(userCourse, cancellationToken);

            var user = await this._usersRepository.GetUserWithMaterialsAsync(email, cancellationToken);
            user.Materials.Remove(user.Materials.FirstOrDefault(m => m.Id == materialId));
            await this._usersRepository.UpdateAsync(user, cancellationToken);

            this._logger.LogInformation($"Material with id: {materialId} added removed from user with " +
                                        $"email: {email}. Material unlearned.");

            var progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);
            return progress;
        }
    }
}
