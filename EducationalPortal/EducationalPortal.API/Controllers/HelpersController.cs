using EducationalPortal.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPortal.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/helpers")]
    public class HelpersController : Controller
    {
        private readonly ICloudStorageService _cloudStorageService;

        public HelpersController(ICloudStorageService cloudStorageService)
        {
            this._cloudStorageService = cloudStorageService;
        }

        [HttpPost("file-to-link/{blobContainer}")]
        [Authorize]
        public async Task<IActionResult> FileToLink(string blobContainer)
        {
            var link = String.Empty;
            var file = Request.Form.Files[0];
            using (var stream = file.OpenReadStream())
            {
                link = await this._cloudStorageService.UploadAsync(stream, file.FileName, file.ContentType,
                                                                   blobContainer);
            }

            return Ok(new { link = link });
        }
    }
}
