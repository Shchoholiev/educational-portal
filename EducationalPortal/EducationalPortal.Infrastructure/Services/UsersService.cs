using EducationalPortal.Application.Descriptions;
using EducationalPortal.Application.DTO;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;
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

        public Task<OperationDetails> UpdateUserAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string id)
        {
            var user = await this._userRepository.GetUserAsync(id); // ?
            await this._userRepository.DeleteAsync(user);
        }
    }
}
