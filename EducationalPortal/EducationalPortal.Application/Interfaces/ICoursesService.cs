﻿using EducationalPortal.Core.Entities;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Interfaces
{
    public interface ICoursesService
    {
        Task AddCourseAsync(Course course);

        Task UpdateCourseAsync(Course course);

        Task DeleteCourseAsync(int id);

        Task<Course> GetCourseAsync(int id);

        Task<IEnumerable<Course>> GetPageAsync(int pageSize, int pageNumber);

        Task<IEnumerable<Course>> GetPageAsync(int pageSize, int pageNumber,
                                               Expression<Func<Course, bool>> predicate);

        Task<IEnumerable<Skill>> GetSkillsAsync();

        Task<int> GetCountAsync();
    }
}
