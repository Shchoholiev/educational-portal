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
        public async Task<ActionResult<IEnumerable<VideoDto>>> GetVideos([FromQuery]PageParameters pageParameters)
        {
            var videos = await this._videosService.GetPageAsync(pageParameters);
            this.SetPagingMetadata(videos);
            return videos;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] VideoCreateDto videoDTO)
        {
            await this._videosService.CreateAsync(videoDTO);
            return StatusCode(201);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await this._videosService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("get-qualities")]
        public async Task<ActionResult<IEnumerable<QualityDto>>> GetQualities()
        {
            return Ok(await this._videosService.GetQualitiesAsync());
        }
    }
}
