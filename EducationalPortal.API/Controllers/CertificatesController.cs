using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Models.DTO.FinalTasks;
using EducationalPortal.Application.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EducationalPortal.API.Controllers
{
    //[Authorize]
    public class CertificatesController : ApiControllerBase
    {
        private readonly ICertificatesService _certificatesService;

        public CertificatesController(ICertificatesService certificatesService)
        {
            this._certificatesService = certificatesService;
        }

        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetPdfAsync(int courseId, CancellationToken cancellationToken)
        {
            var bytes = await this._certificatesService.GetPdfAsync(courseId, UserId, cancellationToken);
            var result = new FileContentResult(bytes, "application/pdf");
            Response.Headers.Add("Content-Disposition", $"inline; filename=Certificate - {User.Identity.Name}.pdf");
            return result;
        }

        [HttpGet("verify/{verificationCode}")]
        public async Task<IActionResult> VerifyAsync(Guid verificationCode, CancellationToken cancellationToken)
        {
            var bytes = await this._certificatesService.VerifyAsync(verificationCode, cancellationToken);
            var result = new FileContentResult(bytes, "application/pdf");
            Response.Headers.Add("Content-Disposition", $"inline; filename=Certificate - {verificationCode}.pdf");
            return result;
        }
    }
}
