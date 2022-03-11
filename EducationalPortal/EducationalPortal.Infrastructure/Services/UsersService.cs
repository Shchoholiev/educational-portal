using EducationalPortal.Application.Descriptions;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities;

namespace EducationalPortal.Infrastructure.Services
{
    public class UsersService : IUserService
    {
        private readonly IUsersRepository _userRepository;

        public UsersService(IUsersRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public Task<OperationDetails> Register(User user)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetails> Login(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetails> UpdateUser(int id)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
