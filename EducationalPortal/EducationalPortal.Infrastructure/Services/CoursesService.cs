using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;

namespace EducationalPortal.Infrastructure.Services
{
    public class CoursesService : ICoursesService
    {
        private readonly IGenericRepository<Course> _coursesRepository;

        private readonly IGenericRepository<Skill> _skillsRepository;

        public CoursesService(IGenericRepository<Course> coursesRepository,
                              IGenericRepository<Skill> skillsRepository)
        {
            this._coursesRepository = coursesRepository;
            this._skillsRepository = skillsRepository;
        }

        public async Task AddCourse(Course course)
        {
            this._coursesRepository.Attach(course);
            await this._coursesRepository.AddAsync(course);
        }
         
        public async Task DeleteCourse(int id)
        {
            var course = await this._coursesRepository.GetOneAsync(id);
            await this._coursesRepository.DeleteAsync(course);
        }

        public async Task<Course> GetCourse(int id)
        {
            return await this._coursesRepository.GetOneAsync(id);
        }

        public async Task<IEnumerable<Course>> GetCourses()
        {
            return await this._coursesRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Course>> GetPage(int pageSize, int pageNumber)
        {
            return await this._coursesRepository.GetPageAsync(pageSize, pageNumber);
        }

        public Task<IEnumerable<Skill>> GetSkills()
        {
            return this._skillsRepository.GetAllAsync();
        }

        public async Task UpdateCourse(Course course)
        {
            this._coursesRepository.Attach(course);
            await this._coursesRepository.UpdateAsync(course);
        }
    }
}
