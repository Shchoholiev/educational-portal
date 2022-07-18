using EducationalPortal.Application.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Interfaces.EducationalMaterials;
using EducationalPortal.Application.Models.DTO.EducationalMaterials;
using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;

namespace EducationalPortal.API.Controllers
{
    [Authorize(Roles = "Creator")]
    public class VideosController : ApiControllerBase
    {
        private readonly IVideosService _videosService;

        public VideosController(IVideosService videosService)
        {
            this._videosService = videosService;
        }

        [HttpGet]
        public async Task<IEnumerable<VideoDto>> GetVideosAsync([FromQuery] PageParameters pageParameters, 
                                                                 CancellationToken cancellationToken)
        {
            var videos = await this._videosService.GetPageAsync(pageParameters, cancellationToken);
            this.SetPagingMetadata(videos);
            return videos;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] VideoCreateDto videoDto, 
                                                     CancellationToken cancellationToken)
        {
            await this._videosService.CreateAsync(videoDto, cancellationToken);
            return StatusCode(201);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await this._videosService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpGet("get-qualities")]
        public async Task<IEnumerable<QualityDto>> GetQualities(CancellationToken cancellationToken)
        {
            return await this._videosService.GetQualitiesAsync(cancellationToken);
        }
    }
}
