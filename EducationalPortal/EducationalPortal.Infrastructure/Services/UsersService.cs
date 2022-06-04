using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Paging;
using EducationalPortal.Application.IRepositories;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;
using System.Linq.Expressions;
using EducationalPortal.Application.Models.DTO;

namespace EducationalPortal.Infrastructure.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        private readonly IPasswordHasher _passwordHasher;

        private readonly ICoursesRepository _coursesRepository;

        private readonly IGenericRepository<Role> _rolesRepository;

        public UsersService(IUsersRepository userRepository, IPasswordHasher passwordHasher,
                            ICoursesRepository coursesRepository, IGenericRepository<Role> rolesRepository)
        {
            this._usersRepository = userRepository;
            this._passwordHasher = passwordHasher;
            this._coursesRepository = coursesRepository;
            this._rolesRepository = rolesRepository;
        }

        public async Task RegisterAsync(UserDto userDTO)
        {
            if (await this._usersRepository.GetUserAsync(userDTO.Email) != null)
            {
            }

            var role = await this._rolesRepository.GetOneAsync(1);

            var user = new User
            {
                Id = DateTime.Now.Ticks.ToString(),
                Name = userDTO.Name,
                Email = userDTO.Email,
                Avatar = "https://educationalportal.blob.core.windows.net/avatars/profile_default.jpg",
                Roles = new List<Role> { role },
            };

            try
            {
                user.PasswordHash = this._passwordHasher.Hash(userDTO.Password);
                await this._usersRepository.AddAsync(user);
            }
            catch (Exception e)
            {

            }

        }

        public async Task LoginAsync(UserDto userDTO)
        {
            var user = await this._usersRepository.GetUserAsync(userDTO.Email);

            if (user == null)
            {
            }

            if (!this._passwordHasher.Check(userDTO.Password, user.PasswordHash))
            {

            }
        }

        public async Task<User?> GetUserAsync(string email)
        {
            return await this._usersRepository.GetUserAsync(email);
        }

        public async Task<User?> GetUserWithMaterialsAsync(string email)
        {
            return await this._usersRepository.GetUserWithMaterialsAsync(email);
        }

        public async Task<User?> GetAuthorAsync(string email)
        {
            return await this._usersRepository.GetAuthorAsync(email);
        }

        public async Task UpdateUserAsync(User user)
        {
            await this._usersRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(string email)
        {
            var user = await this._usersRepository.GetUserAsync(email);
            await this._usersRepository.DeleteAsync(user);
        }

        public async Task AddUsersCoursesAsync(UsersCourses usersCourses)
        {
            await this._usersRepository.AddUsersCoursesAsync(usersCourses);
        }

        public async Task<UsersCourses?> GetUsersCoursesAsync(int courseId, string email)
        {
            return await this._usersRepository.GetUsersCoursesAsync(courseId, email);
        }

        public async Task<PagedList<UsersCourses>> GetUsersCoursesPageAsync(string email, PageParameters pageParameters, 
                                                                Expression<Func<UsersCourses, bool>> predicate)
        {
            var usersCourses = await this._usersRepository
                                         .GetUsersCoursesPageAsync(email, pageParameters, predicate);
            foreach (var uc in usersCourses)
            {
                var learnedMaterialsCount = uc.LearnedMaterialsCount;
                uc.LearnedMaterialsCount = await this._usersRepository
                                                     .GetLearnedMaterialsCountAsync(uc.CourseId, email);
                await this._usersRepository.UpdateUsersCoursesAsync(uc);
                if (learnedMaterialsCount != uc.LearnedMaterialsCount
                    && uc.LearnedMaterialsCount == uc.MaterialsCount)
                {
                    await this.AddAcquiredSkills(uc.CourseId, email);
                }
            }

            return usersCourses;
        }

        public async Task UpdateUsersCoursesAsync(UsersCourses usersCourses)
        {
            await this._usersRepository.UpdateUsersCoursesAsync(usersCourses);
        }

        public async Task AddAcquiredSkills(int courseId, string email)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(courseId);
            var user = await this._usersRepository.GetUserWithSkillsAsync(email);
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

        public async Task<int> GetLearnedMaterialsCountAsync(int courseId, string email)
        {
            return await this._usersRepository.GetLearnedMaterialsCountAsync(courseId, email);
        }

        public async Task SaveDbAsync()
        {
            await this._usersRepository.SaveAsync();
        }
    }
}
