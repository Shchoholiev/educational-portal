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

        public async Task<OperationDetails> Register(UserDTO userDTO)
        {
            var user = new User
            {
                Id = DateTime.Now.Ticks.ToString(),
                Name = userDTO.Name,
                Email = userDTO.Email,
            };

            var operationDetails = new OperationDetails();

            try
            {
                (user.SecurityStamp, user.PasswordHash) = this._passwordHasher.Hash(userDTO.Password);
                await this._userRepository.AddAsync(user);
            }
            catch (Exception e)
            {
                operationDetails.AddError(e.Message);
            }

            return operationDetails;
        }

        public async Task<OperationDetails> Login(UserDTO userDTO)
        {
            var user = await this._userRepository.GetUserAsync(userDTO.Email);

            var operationDetails = new OperationDetails();
            if (user == null)
            {
                operationDetails.AddError("User with this email not found!");
            }

            if (!this._passwordHasher.Check(userDTO.Password, user.PasswordHash, user.SecurityStamp))
            {
                operationDetails.AddError("Incorrect password!");
            }

            return operationDetails;
        }

        public Task<User> GetUser(string id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetails> UpdateUser(string id)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(string id)
        {
            var user = await this._userRepository.GetUserAsync(id); // ?
            await this._userRepository.DeleteAsync(user);
        }
    }
}
