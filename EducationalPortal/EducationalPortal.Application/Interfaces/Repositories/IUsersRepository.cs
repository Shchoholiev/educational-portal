using EducationalPortal.Core.Entities;

namespace EducationalPortal.Application.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        Task AddAsync(User user);

        Task UpdateAsync(User user);

        Task DeleteAsync(User user);

        Task<User?> GetUserAsync(string email);

        Task<User?> GetUserWithSkillsAsync(string email);

        Task<User?> GetUserWithMaterialsAsync(string email);

        Task<User?> GetAuthorAsync(string email);

        Task SaveAsync();
    }
}
