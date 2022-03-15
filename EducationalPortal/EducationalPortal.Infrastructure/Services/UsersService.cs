using EducationalPortal.Application.Descriptions;
using EducationalPortal.Application.DTO;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Infrastructure.Identity;

namespace EducationalPortal.Infrastructure.Services
{
    public class UsersService : IUserService
    {
        private readonly IUsersRepository _userRepository;

        private readonly IPasswordHasher _passwordHasher;

        public UsersService(IUsersRepository userRepository, IPasswordHasher passwordHasher)
        {
            this._userRepository = userRepository;
            this._passwordHasher = passwordHasher;
        }

        public async Task<OperationDetails> RegisterAsync(UserDTO userDTO)
        {
            var operationDetails = new OperationDetails();
            if (await this._userRepository.GetUserAsync(userDTO.Email) != null)
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
                await this._userRepository.AddAsync(user);
            }
            catch (Exception e)
            {
                operationDetails.AddError(e.Message);
            }

            return operationDetails;
        }

        public async Task<OperationDetails> LoginAsync(UserDTO userDTO)
        {
            var user = await this._userRepository.GetUserAsync(userDTO.Email);

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
            return await this._userRepository.GetUserAsync(email);
        }

        public async Task<User?> GetUserWithSkillsAsync(string email)
        {
            return await this._userRepository.GetUserWithSkillsAsync(email);
        }

        public async Task<IEnumerable<UsersCourses>> GetUsersCoursesPageAsync(string email, 
                                                                              int pageSize, int pageNumber)
        {
            return await this._userRepository.GetUsersCoursesPageAsync(email, pageSize, pageNumber);
        }

        public async Task<int> GetUsersCoursesCountAsync(string email)
        {
            return await this._userRepository.GetUsersCoursesCountAsync(email);
        }

        public async Task<OperationDetails> UpdateUserAsync(User user)
        {
            await this._userRepository.UpdateAsync(user);
            return new OperationDetails();
        }

        public async Task DeleteAsync(string email)
        {
            var user = await this._userRepository.GetUserAsync(email); // ?
            await this._userRepository.DeleteAsync(user);
        }
    }
}
