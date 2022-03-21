using EducationalPortal.Application.Descriptions;
using EducationalPortal.Application.DTO;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Infrastructure.Identity;

namespace EducationalPortal.Infrastructure.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        private readonly IPasswordHasher _passwordHasher;

        private readonly ICoursesRepository _coursesRepository;

        public UsersService(IUsersRepository userRepository, IPasswordHasher passwordHasher,
                            ICoursesRepository coursesRepository)
        {
            this._usersRepository = userRepository;
            this._passwordHasher = passwordHasher;
            this._coursesRepository = coursesRepository;
        }

        public async Task<OperationDetails> RegisterAsync(UserDTO userDTO)
        {
            var operationDetails = new OperationDetails();
            if (await this._usersRepository.GetUserAsync(userDTO.Email) != null)
            {
                operationDetails.AddError("This email is already used!");
                return operationDetails;
            }

            var user = new User
            {
                Id = DateTime.Now.Ticks.ToString(),
                Name = userDTO.Name,
                Email = userDTO.Email,
                Avatar = "https://educationalportal.blob.core.windows.net/avatars/profile_default.jpg",
            };

            try
            {
                user.PasswordHash = this._passwordHasher.Hash(userDTO.Password);
                await this._usersRepository.AddAsync(user);
            }
            catch (Exception e)
            {
                operationDetails.AddError(e.Message);
            }

            return operationDetails;
        }

        public async Task<OperationDetails> LoginAsync(UserDTO userDTO)
        {
            var user = await this._usersRepository.GetUserAsync(userDTO.Email);

            var operationDetails = new OperationDetails();
            if (user == null)
            {
                operationDetails.AddError("User with this email not found!");
                return operationDetails;
            }

            if (!this._passwordHasher.Check(userDTO.Password, user.PasswordHash))
            {
                operationDetails.AddError("Incorrect password!");
            }

            return operationDetails;
        }

        public async Task<User?> GetUserAsync(string email)
        {
            return await this._usersRepository.GetUserAsync(email);
        }

        public async Task<User?> GetUserWithSkillsAsync(string email)
        {
            return await this._usersRepository.GetUserWithSkillsAsync(email);
        }

        public async Task<User?> GetUserWithMaterialsAsync(string email)
        {
            return await this._usersRepository.GetUserWithMaterialsAsync(email);
        }

        public async Task<User?> GetAuthorAsync(string email)
        {
            return await this._usersRepository.GetAuthorAsync(email);
        }

        public async Task<OperationDetails> UpdateUserAsync(User user)
        {
            await this._usersRepository.UpdateAsync(user);
            return new OperationDetails();
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

        public async Task<IEnumerable<UsersCourses>> GetUsersCoursesPageAsync(string email, 
                                                                              int pageSize, int pageNumber)
        {
            var usersCourses = await this._usersRepository.GetUsersCoursesPageAsync(email, pageSize, pageNumber);
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

        public async Task<int> GetUsersCoursesCountAsync(string email)
        {
            return await this._usersRepository.GetUsersCoursesCountAsync(email);
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
    }
}
