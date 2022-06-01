using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EducationalPortal.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace EducationalPortal.Infrastructure.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly string _connectionString;

        public CloudStorageService(IConfiguration configuration)
        {
            this._connectionString = configuration.GetConnectionString("AzureStorage");
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName, string fileType, 
                                              string containerName)
        {
            var container = new BlobContainerClient(this._connectionString, containerName);
            var blob = container.GetBlobClient(fileName);

            while (await blob.ExistsAsync())
            {
                fileName = fileName + "_new";
                blob = container.GetBlobClient(fileName);
            }
            
            await blob.UploadAsync(fileStream, new BlobHttpHeaders { ContentType = fileType });

            return blob.Uri.ToString();
        }
        
        public async Task DeleteAsync(string fileLink, string containerName)
        {
            var container = new BlobContainerClient(_connectionString, containerName);
            var regex = new Regex($".*{containerName}/");
            var filePath = regex.Split(fileLink);
            await container.DeleteBlobAsync(filePath[1]);
        }
    }
}
