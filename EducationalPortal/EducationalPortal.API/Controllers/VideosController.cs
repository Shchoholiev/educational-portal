//using EducationalPortal.Application.DTO;
//using EducationalPortal.Application.Interfaces;
//using EducationalPortal.Application.Paging;
//using EducationalPortal.Application.Repository;
//using EducationalPortal.Core.Entities.EducationalMaterials;
//using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
//using EducationalPortal.API.Mapping;
//using EducationalPortal.API.ViewModels.CreateViewModels;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace EducationalPortal.API.Controllers
//{
//    public class VideosController : Controller
//    {
//        private readonly IGenericRepository<Video> _videosRepository;

//        private readonly IGenericRepository<Quality> _qualitiesRepository;

//        private readonly ICloudStorageService _cloudStorageService;

//        private readonly Mapper _mapper = new();

//        public VideosController(IGenericRepository<Video> videosRepository,
//                                IGenericRepository<Quality> qualitiesRepository,
//                                ICloudStorageService cloudStorageService)
//        {
//            this._videosRepository = videosRepository;
//            this._qualitiesRepository = qualitiesRepository;
//            this._cloudStorageService = cloudStorageService;
//        }


//        [HttpGet]
//        [Authorize(Roles = "Creator")]
//        public async Task<PartialViewResult> Index(PageParameters pageParameters, List<Video> chosenVideos)
//        {
//            pageParameters.PageSize = 8;
//            var videos = await this._videosRepository.GetPageAsync(pageParameters, v => v.Quality);
//            var videoViewModels = this._mapper.Map(videos, chosenVideos);
//            var totalCount = await this._videosRepository.GetCountAsync(v => true);
//            var pagedVideos = new PagedList<VideoCreateModel>(videoViewModels, pageParameters, totalCount);

//            return PartialView("_AddVideosPanel", pagedVideos);
//        }

//        [HttpGet]
//        [Authorize(Roles = "Creator")]
//        public async Task<PartialViewResult> GetQualities()
//        {
//            var qualities = await this._qualitiesRepository.GetAllAsync();
//            return PartialView("_Qualities", qualities);
//        }

//        [HttpPost]
//        [Authorize(Roles = "Creator")]
//        public async Task<IActionResult> Create(VideoDTO videoDTO)
//        {
//            if (ModelState.IsValid)
//            {
//                if (await this._videosRepository.Exists(v => v.Name == videoDTO.Name))
//                {
//                    ModelState.AddModelError(string.Empty, "Video with this name already exists!");
//                }
//                else
//                {
//                    var video = new Video { Name = videoDTO.Name };
//                    using (var stream = videoDTO.File.OpenReadStream())
//                    {
//                        video.Link = await this._cloudStorageService.UploadAsync(stream, videoDTO.File.FileName,
//                                                                            videoDTO.File.ContentType, "videos");
//                    }
//                    video.Duration = DateTime.MinValue.AddSeconds(videoDTO.Duration);
//                    video.Quality = new Quality { Id = videoDTO.Quality.Id, Name = videoDTO.Quality.Name ?? null };
//                    this._videosRepository.Attach(video);
//                    await this._videosRepository.AddAsync(video);
//                    return Json(new { success = true });
//                }
//            }

//            return PartialView("_CreateVideo", videoDTO);
//        }

//        [HttpPost]
//        [Authorize(Roles = "Creator")]
//        public async Task<IActionResult> Add(int idVideos, List<MaterialsBase> materials)
//        {
//            var video = await this._videosRepository.GetOneAsync(idVideos);
//            materials.Add(video);
//            return PartialView("_Materials", materials);
//        }

//        [HttpPost]
//        [Authorize(Roles = "Creator")]
//        public IActionResult Remove(int idVideos, List<MaterialsBase> materials)
//        {
//            materials.Remove(materials.FirstOrDefault(s => s.Id == idVideos));
//            return PartialView("_Materials", materials);
//        }

//        [HttpPost]
//        [Authorize(Roles = "Creator")]
//        public async Task<IActionResult> Delete(int idVideos)
//        {
//            if (await this._videosRepository.Exists(v => v.CoursesMaterials.Any(cm => cm.MaterialId == idVideos)))
//            {
//                return Json(new { success = false, message = "This video is used in other courses!" });
//            }
//            else
//            {
//                var video = await this._videosRepository.GetOneAsync(idVideos);
//                await this._videosRepository.DeleteAsync(video);
//                return Json(new { success = true });
//            }
//        }

//        [Authorize(Roles = "Student")]
//        public async Task<PartialViewResult> Video(int id)
//        {
//            var video = await this._videosRepository.GetOneAsync(id, v => v.Quality);
//            return PartialView("_Video", video);
//        }
//    }
//}
