namespace EducationalPortal.Application.Interfaces
{
    public interface ICloudStorageService
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string fileType, string containerName, 
                                 CancellationToken cancellationToken);

        Task DeleteAsync(string fileName, string containerName, CancellationToken cancellationToken);
    }
}
