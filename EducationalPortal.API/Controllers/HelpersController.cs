﻿using EducationalPortal.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPortal.API.Controllers
{
    [Authorize]
    public class HelpersController : ApiControllerBase
    {
        private readonly ICloudStorageService _cloudStorageService;

        public HelpersController(ICloudStorageService cloudStorageService)
        {
            this._cloudStorageService = cloudStorageService;
        }

        [HttpPost("file-to-link/{blobContainer}")]
        [Authorize]
        public async Task<IActionResult> FileToLinkAsync(string blobContainer, [FromForm] IFormFile file, 
                                                         CancellationToken cancellationToken)
        {
            using (var stream = file.OpenReadStream())
            {
                var link = await this._cloudStorageService.UploadAsync(stream, 
                    Path.GetFileNameWithoutExtension(file.FileName), file.ContentType, blobContainer, cancellationToken);
                return Ok(new { link });
            }
        }
    }
}
