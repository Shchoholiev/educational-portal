using EducationalPortal.Core.Entities.Courses;

namespace EducationalPortal.Application.Interfaces.Repositories
{
    public interface ICertificatesRepository
    {
        Task AddAsync(Certificate certificate, CancellationToken cancellationToken);

        Task<Certificate?> GetCertificateAsync(int courseId, string userId, CancellationToken cancellationToken);

        Task<Certificate?> GetCertificateAsync(Guid verificationCode, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(int courseId, string userId, CancellationToken cancellationToken);
    }
}
