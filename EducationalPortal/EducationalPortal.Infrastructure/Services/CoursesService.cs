using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.JoinEntities;
using System.Linq.Expressions;

namespace EducationalPortal.Infrastructure.Services
{
    public class CoursesService : ICoursesService
    {
        private readonly ICoursesRepository _coursesRepository;

        private readonly IGenericRepository<Skill> _skillsRepository;

        private readonly IUsersRepository _usersRepository;

        public CoursesService(ICoursesRepository coursesRepository, IUsersRepository usersRepository,
                              IGenericRepository<Skill> skillsRepository,
                              IVideosService videosService, IBooksService booksService,
                              IArticlesService articlesService)
        {
            this._coursesRepository = coursesRepository;
            this._usersRepository = usersRepository;
            this._skillsRepository = skillsRepository;
        }

        public async Task AddCourseAsync(Course course)
        {
            this._coursesRepository.Attach(course);
            await this._coursesRepository.AddAsync(course);
        }

        public async Task UpdateCourseAsync(Course course)
        {
            this._coursesRepository.Attach(course);
            await this._coursesRepository.UpdateAsync(course);
        }

        public async Task DeleteCourseAsync(int id)
        {
            var course = await this._coursesRepository.GetCourseAsync(id);
            await this._coursesRepository.DeleteAsync(course);
        }

        public async Task<Course> GetCourseAsync(int id)
        {
            return await this._coursesRepository.GetFullCourseAsync(id);
        }

        public async Task<IEnumerable<Course>> GetPageAsync(int pageSize, int pageNumber)
        {
            return await this._coursesRepository.GetPageAsync(pageSize, pageNumber);
        }

        public async Task<IEnumerable<Course>> GetPageAsync(int pageSize, int pageNumber,
                                                            Expression<Func<Course, bool>> predicate)
        {
            return await this._coursesRepository.GetPageAsync(pageSize, pageNumber, predicate);
        }

        public async Task<int> GetCountAsync()
        {
            return await this._coursesRepository.GetCountAsync();
        }

        public async Task<IEnumerable<Skill>> GetSkillsAsync()
        {
            return await this._skillsRepository.GetAllAsync();
        }

        public async Task<UsersCourses?> GetUsersCoursesAsync(int courseId, string email)
        {
            return await this._usersRepository.GetUsersCoursesAsync(courseId, email);
        }
    }
}
