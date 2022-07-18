using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EducationalPortal.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace EducationalPortal.Infrastructure.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly string _connectionString;

        private readonly ILogger _logger;

        public CloudStorageService(IConfiguration configuration, ILogger<CloudStorageService> logger)
        {
            this._connectionString = configuration.GetConnectionString("AzureStorage");
            this._logger = logger;
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName, string fileType, 
                                              string containerName, CancellationToken cancellationToken)
        {
            var container = new BlobContainerClient(this._connectionString, containerName);
            var blob = container.GetBlobClient(fileName);

            while (await blob.ExistsAsync(cancellationToken))
            {
                this._logger.LogInformation($"File with name: {fileName} already exists. " +
                                            $"Changing name and trying upload file again.");

                fileName += "_new";
                blob = container.GetBlobClient(fileName);
            }

            var options = new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = fileType } };
            await blob.UploadAsync(fileStream, options, cancellationToken);
            var link = blob.Uri.ToString();

            this._logger.LogInformation($"Uploaded file to Blob Storage. Link: {link}.");

            return link;
        }
        
        public async Task DeleteAsync(string fileLink, string containerName, CancellationToken cancellationToken)
        {
            var container = new BlobContainerClient(_connectionString, containerName);
            var regex = new Regex($".*{containerName}/");
            var filePath = regex.Split(fileLink);
            await container.DeleteBlobAsync(filePath[1], DeleteSnapshotsOption.None, null, cancellationToken);

            this._logger.LogInformation($"Deleted file from Blob Storage via link: {fileLink}.");
        }
    }
}
