﻿using EducationalPortal.Core.Entities;
using System.Linq.Expressions;

namespace EducationalPortal.Application.Interfaces.Repositories
{
    public interface IUsersRepository
    {
        Task AddAsync(User user, CancellationToken cancellationToken);

        Task UpdateAsync(User user, CancellationToken cancellationToken);

        Task DeleteAsync(User user, CancellationToken cancellationToken);

        Task<User?> GetUserAsync(string userId, CancellationToken cancellationToken);

        Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);

        Task<User?> GetUserWithSkillsAsync(string userId, CancellationToken cancellationToken);

        Task<User?> GetUserWithMaterialsAsync(string userId, CancellationToken cancellationToken);

        Task<User?> GetAuthorAsync(string email, CancellationToken cancellationToken);

        Task AddAcquiredSkillsAsync(int courseId, string userId, CancellationToken cancellationToken);
    
        Task<bool> ExistsAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken);
    }
}
