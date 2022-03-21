using EducationalPortal.Application.Descriptions;
using EducationalPortal.Application.DTO;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.JoinEntities;

namespace EducationalPortal.Application.Interfaces
{
    public interface IUsersService
    {
        Task<OperationDetails> RegisterAsync(UserDTO userDTO);

        Task<OperationDetails> LoginAsync(UserDTO userDTO);

        Task<OperationDetails> UpdateUserAsync(User user);

        Task DeleteAsync(string id);

        Task<User?> GetUserAsync(string email);

        Task<User?> GetUserWithSkillsAsync(string email);

        Task<User?> GetUserWithMaterialsAsync(string email);

        Task AddUsersCoursesAsync(UsersCourses usersCourses);

        Task<UsersCourses?> GetUsersCoursesAsync(int courseId, string email);

        Task<IEnumerable<UsersCourses>> GetUsersCoursesPageAsync(string email, int pageSize, int pageNumber);

        Task UpdateUsersCoursesAsync(UsersCourses usersCourses);

        Task<int> GetUsersCoursesCountAsync(string email);

        Task AddAcquiredSkills(int courseId, string email);

        Task<int> GetLearnedMaterialsCountAsync(int courseId, string email);
    }
}
