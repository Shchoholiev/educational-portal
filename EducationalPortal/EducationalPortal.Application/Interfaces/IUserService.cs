using EducationalPortal.Application.Descriptions;
using EducationalPortal.Application.DTO;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;

namespace EducationalPortal.Application.Interfaces
{
    public interface IUserService
    {
        Task<OperationDetails> RegisterAsync(UserDTO userDTO);

        Task<OperationDetails> LoginAsync(UserDTO userDTO);

        Task<OperationDetails> UpdateUserAsync(User user);

        Task DeleteAsync(string id);

        Task<User?> GetUserAsync(string email);

        Task<User?> GetUserWithSkillsAsync(string email);

        Task<IEnumerable<UsersCourses>> GetUsersCoursesPageAsync(string email, int pageSize, int pageNumber);

        Task<int> GetUsersCoursesCountAsync(string email);
    }
}
