namespace EducationalPortal.Application.Interfaces
{
    public interface ICertificatesService
    {
        Task CreateAsync(int courseId, string userId, CancellationToken cancellationToken);

        Task<Stream> GetPdfAsync(int courseId, string userId, CancellationToken cancellationToken);

        Task<Stream> VerifyAsync(Guid verificationCode, CancellationToken cancellationToken);
    }
}
