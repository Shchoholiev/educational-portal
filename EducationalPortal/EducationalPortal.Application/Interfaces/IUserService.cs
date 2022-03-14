using EducationalPortal.Application.Descriptions;
using EducationalPortal.Application.DTO;
using EducationalPortal.Core.Entities;

namespace EducationalPortal.Application.Interfaces
{
    public interface IUserService
    {
        Task<OperationDetails> RegisterAsync(UserDTO userDTO);

        Task<OperationDetails> LoginAsync(UserDTO userDTO);

        Task DeleteAsync(string id);

        Task<User?> GetUserAsync(string email);

        Task<OperationDetails> UpdateUserAsync(string id);
    }
}
