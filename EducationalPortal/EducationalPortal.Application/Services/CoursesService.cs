﻿using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;

namespace EducationalPortal.Application.Services
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
            await this._coursesRepository.Add(course);
        }

        public async Task DeleteCourse(int id)
        {
            await this._coursesRepository.Delete(id);
        }

        // ???
        public async Task<Course> GetCourse(int id)
        {
            return await this._coursesRepository.GetOne(id, 
                c => c.CoursesMaterials.Select(cm => cm.Material));
        }

        public async Task<IEnumerable<Course>> GetCourses()
        {
            return await this._coursesRepository.GetAll(c => c.CoursesMaterials.Select(cm => cm.Material));
        }

        public async Task<IEnumerable<Course>> GetPage(int pageSize, int pageNumber)
        {
            return await this._coursesRepository.GetPage(pageSize, pageNumber,
                c => c.CoursesMaterials.Select(cm => cm.Material));
        }

        public Task<IEnumerable<Skill>> GetSkills()
        {
            return this._skillsRepository.GetAll();
        }

        public async Task UpdateCourse(Course course)
        {
            this._coursesRepository.Attach(course);
            await this._coursesRepository.Update(course);
        }
    }
}
