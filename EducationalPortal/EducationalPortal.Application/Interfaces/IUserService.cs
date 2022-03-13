using EducationalPortal.Application.Descriptions;
using EducationalPortal.Application.DTO;
using EducationalPortal.Core.Entities;

namespace EducationalPortal.Application.Interfaces
{
    public interface IUserService
    {
        Task<OperationDetails> Register(UserDTO userDTO);

        Task<OperationDetails> Login(UserDTO userDTO);

        Task Delete(string id);

        Task<User> GetUser(string id);

        Task<OperationDetails> UpdateUser(string id);
    }
}
