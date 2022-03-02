using EducationalPortal.Application.Descriptions;
using EducationalPortal.Core.Entities;

namespace EducationalPortal.Application.Interfaces
{
    public interface IUserService
    {
        Task<OperationDetails> Register(User user);

        Task<OperationDetails> Login(User user);

        Task Delete(int id);

        Task<User> GetUser(int id);

        Task<OperationDetails> UpdateUser(int id);
    }
}
