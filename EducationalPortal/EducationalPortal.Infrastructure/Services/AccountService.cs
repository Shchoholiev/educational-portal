﻿using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Interfaces.Identity;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Mapping;
using EducationalPortal.Application.Models;
using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities.JoinEntities;
using System.Linq.Expressions;

namespace EducationalPortal.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUsersRepository _usersRepository;

        private readonly IUsersCoursesRepository _usersCoursesRepository;

        private readonly ICoursesRepository _coursesRepository;

        private readonly IUserManager _userManager;

        private readonly Mapper _mapper = new();

        public AccountService(IUsersRepository userRepository, IUsersCoursesRepository usersCoursesRepository,
                              ICoursesRepository coursesRepository, IUserManager userManager)
        {
            this._usersRepository = userRepository;
            this._usersCoursesRepository = usersCoursesRepository;
            this._coursesRepository = coursesRepository;
            this._userManager = userManager;
        }

        public async Task<TokensModel> RegisterAsync(RegisterModel register)
        {
            return await this._userManager.RegisterAsync(register);
        }

        public async Task<TokensModel> LoginAsync(LoginModel login)
        {
            return await this._userManager.LoginAsync(login);
        }

        public async Task<TokensModel> UpdateAsync(string email, UserDto userDto)
        {
            return await this._userManager.UpdateAsync(email, userDto);
        }

        public async Task DeleteAsync(string email)
        {
            var user = await this._usersRepository.GetUserAsync(email);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            await this._usersRepository.DeleteAsync(user);
        }

        public async Task<TokensModel> AddToRoleAsync(string roleName, string email)
        {
            return await this._userManager.AddToRoleAsync(roleName, email);
        }

        public async Task<UserDto> GetUserAsync(string email)
        {
            var user = await this._usersRepository.GetAuthorAsync(email);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            var dto = this._mapper.Map(user);
            return dto;
        }

        public async Task<UserDto> GetAuthorAsync(string email)
        {
            var author = await this._usersRepository.GetAuthorAsync(email);
            if (author == null)
            {
                throw new NotFoundException("User");
            }

            if (!author.Roles.Any(r => r.Name == "Creator"))
            {
                throw new NotFoundException("Author");
            }

            var dto = this._mapper.Map(author);
            return dto;
        }

        public async Task<PagedList<UsersCourses>> GetUsersCoursesPageAsync(string email, PageParameters pageParameters, 
                                                                Expression<Func<UsersCourses, bool>> predicate)
        {
            var usersCourses = await this._usersCoursesRepository
                                         .GetUsersCoursesPageAsync(email, pageParameters, predicate);
            foreach (var uc in usersCourses)
            {
                var learnedMaterialsCount = uc.LearnedMaterialsCount;
                uc.LearnedMaterialsCount = await this._usersCoursesRepository
                                                     .GetLearnedMaterialsCountAsync(uc.CourseId, email);
                await this._usersCoursesRepository.UpdateUsersCoursesAsync(uc);
                if (learnedMaterialsCount != uc.LearnedMaterialsCount
                    && uc.LearnedMaterialsCount == uc.MaterialsCount)
                {
                    await this.AddAcquiredSkillsAsync(uc.CourseId, email);
                }
            }

            return usersCourses;
        }

        public async Task AddAcquiredSkillsAsync(int courseId, string email)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(courseId);
            if (course == null)
            {
                throw new NotFoundException("Course");
            }

            var user = await this._usersRepository.GetUserWithSkillsAsync(email);
            if (user == null)
            {
                throw new NotFoundException("User");
            }

            foreach (var skill in course.Skills)
            {
                if (user.UsersSkills.Any(us => us.SkillId == skill.Id))
                {
                    user.UsersSkills.FirstOrDefault(us => us.SkillId == skill.Id).Level++;
                }
                else
                {
                    user.UsersSkills.Add(new UsersSkills
                    {
                        SkillId = skill.Id,
                        Level = 1,
                    });
                }
            }

            await this._usersRepository.UpdateAsync(user);
        }
    }
}
