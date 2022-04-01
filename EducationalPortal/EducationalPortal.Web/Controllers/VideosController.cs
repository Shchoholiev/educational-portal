using EducationalPortal.Application.DTO;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using EducationalPortal.Web.Mapping;
using EducationalPortal.Web.Paging;
using EducationalPortal.Web.ViewModels.CreateViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationalPortal.Web.Controllers
{
    [Authorize(Roles = "Creator")]
    public class VideosController : Controller
    {
        private readonly IGenericRepository<Video> _videosRepository;

        private readonly IGenericRepository<Quality> _qualitiesRepository;

        private readonly ICloudStorageService _cloudStorageService;

        private readonly Mapper _mapper = new();

        public VideosController(IGenericRepository<Video> videosRepository,
                                IGenericRepository<Quality> qualitiesRepository,
                                ICloudStorageService cloudStorageService)
        {
            this._videosRepository = videosRepository;
            this._qualitiesRepository = qualitiesRepository;
            this._cloudStorageService = cloudStorageService;
        }


        [HttpGet]
        public async Task<PartialViewResult> Index(PageParameters pageParameters, List<Video> chosenVideos)
        {
            pageParameters.PageSize = 8;
            var videos = await this._videosRepository
                                   .GetPageAsync(pageParameters.PageSize, pageParameters.PageNumber,
                                                 v => v.Quality);
            var videoViewModels = this._mapper.Map(videos, chosenVideos);
            var totalCount = await this._videosRepository.GetCountAsync(v => true);
            var pagedVideos = new PagedList<VideoCreateModel>(videoViewModels, pageParameters, totalCount);

            return PartialView("_AddVideosPanel", pagedVideos);
        }

        [HttpGet]
        public async Task<PartialViewResult> GetQualities()
        {
            var qualities = await this._qualitiesRepository.GetAllAsync();
            return PartialView("_Qualities", qualities);
        }

        [HttpPost]
        public async Task<IActionResult> Create(VideoDTO videoDTO)
        {
            if (ModelState.IsValid)
            {
                if ((await this._videosRepository.GetAllAsync(v => v.Name == videoDTO.Name)).Count() > 0)
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
                    return Json(new { success = true });
                }
            }

            return PartialView("_CreateVideo", videoDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int idVideos, List<MaterialsBase> materials)
        {
            var video = await this._videosRepository.GetOneAsync(idVideos);
            materials.Add(video);
            return PartialView("_Materials", materials);
        }

        [HttpPost]
        public IActionResult Remove(int idVideos, List<MaterialsBase> materials)
        {
            materials.Remove(materials.FirstOrDefault(s => s.Id == idVideos));
            return PartialView("_Materials", materials);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int idVideos)
        {
            if ((await this._videosRepository.GetCountAsync(v => v.CoursesMaterials
                                                            .Any(cm => cm.MaterialId == idVideos))) == 0)
            {
                var video = await this._videosRepository.GetOneAsync(idVideos);
                await this._videosRepository.DeleteAsync(video);
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "This video is used in other courses!" });
            }
        }

        [Authorize(Roles = "Student")]
        public async Task<PartialViewResult> Video(int id)
        {
            var video = await this._videosRepository.GetOneAsync(id, v => v.Quality);
            return PartialView("_Video", video);
        }
    }
}
