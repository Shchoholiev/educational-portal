using EducationalPortal.Core.Entities;

namespace EducationalPortal.Application.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        Task AddAsync(User user, CancellationToken cancellationToken);

        Task UpdateAsync(User user, CancellationToken cancellationToken);

        Task DeleteAsync(User user, CancellationToken cancellationToken);

        Task<User?> GetUserAsync(string email, CancellationToken cancellationToken);

        Task<User?> GetUserWithSkillsAsync(string email, CancellationToken cancellationToken);

        Task<User?> GetUserWithMaterialsAsync(string email, CancellationToken cancellationToken);

        Task<User?> GetAuthorAsync(string email, CancellationToken cancellationToken);

        Task AddAcquiredSkillsAsync(int courseId, string email, CancellationToken cancellationToken);
    }
}
