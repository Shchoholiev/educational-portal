namespace EducationalPortal.Application.Interfaces
{
    public interface ICertificatesService
    {
        Task CreateAsync(int courseId, string userId, CancellationToken cancellationToken);

        Task<byte[]> GetPdfAsync(int courseId, string userId, CancellationToken cancellationToken);

        Task<byte[]> VerifyAsync(Guid verificationCode, CancellationToken cancellationToken);

        Task<bool> ExistsAsync(int courseId, string userId, CancellationToken cancellationToken);
    }
}
