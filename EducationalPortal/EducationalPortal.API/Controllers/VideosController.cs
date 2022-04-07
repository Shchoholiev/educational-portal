using EducationalPortal.Application.DTO;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Paging;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EducationalPortal.API.Controllers
{
    [Authorize(Roles = "Creator")]
    [ApiController]
    [Route("api/videos")]
    public class VideosController : Controller
    {
        private readonly IGenericRepository<Video> _videosRepository;

        private readonly IGenericRepository<Quality> _qualitiesRepository;

        private readonly ICloudStorageService _cloudStorageService;

        public VideosController(IGenericRepository<Video> videosRepository,
                                IGenericRepository<Quality> qualitiesRepository,
                                ICloudStorageService cloudStorageService)
        {
            this._videosRepository = videosRepository;
            this._qualitiesRepository = qualitiesRepository;
            this._cloudStorageService = cloudStorageService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> GetVideos([FromQuery]PageParameters pageParameters)
        {
            var videos = await this._videosRepository.GetPageAsync(pageParameters, v => v.Quality);
            var metadata = new
            {
                videos.TotalItems,
                videos.PageSize,
                videos.PageNumber,
                videos.TotalPages,
                videos.HasNextPage,
                videos.HasPreviousPage
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return videos;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]VideoDTO videoDTO)
        {
            if (ModelState.IsValid)
            {
                if (await this._videosRepository.Exists(v => v.Name == videoDTO.Name))
                {
                    ModelState.AddModelError(string.Empty, "Video with this name already exists!");
                }
                else
                {
                    var video = new Video { Name = videoDTO.Name };
                    using (var stream = videoDTO.File.OpenReadStream())
                    {
                        video.Link = await this._cloudStorageService.UploadAsync(stream, videoDTO.File.FileName,
                                                                            videoDTO.File.ContentType, "videos");
                    }
                    video.Duration = DateTime.MinValue.AddSeconds(videoDTO.Duration);
                    video.Quality = new Quality { Id = videoDTO.Quality.Id, Name = videoDTO.Quality.Name ?? null };
                    this._videosRepository.Attach(video);
                    await this._videosRepository.AddAsync(video);
                    return StatusCode(201);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await this._videosRepository.Exists(v => v.CoursesMaterials.Any(cm => cm.MaterialId == id)))
            {
                return BadRequest("This video is used in other videos!");
            }
            else
            {
                var video = await this._videosRepository.GetOneAsync(id);
                if (video == null)
                {
                    return NotFound();
                }

                await this._videosRepository.DeleteAsync(video);
                return NoContent();
            }
        }

        [HttpGet("get-qualities")]
        public async Task<ActionResult<IEnumerable<Quality>>> GetQualities()
        {
            var qualities = (await this._qualitiesRepository.GetAllAsync()).ToList();
            return qualities;
        }
    }
}
