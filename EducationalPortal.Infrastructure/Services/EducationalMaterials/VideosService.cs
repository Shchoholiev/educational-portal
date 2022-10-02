using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Interfaces.EducationalMaterials;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Mapping;
using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.EducationalMaterials;
using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using Microsoft.Extensions.Logging;

namespace EducationalPortal.Infrastructure.Services.EducationalMaterials
{
    public class VideosService : IVideosService
    {
        private readonly IGenericRepository<Video> _videosRepository;

        private readonly IGenericRepository<Quality> _qualitiesRepository;

        private readonly ICloudStorageService _cloudStorageService;

        private readonly ILogger _logger;

        private readonly Mapper _mapper = new();

        public VideosService(IGenericRepository<Video> videosRepository,
                                IGenericRepository<Quality> qualitiesRepository,
                                ICloudStorageService cloudStorageService, ILogger<VideosService> logger)
        {
            this._videosRepository = videosRepository;
            this._qualitiesRepository = qualitiesRepository;
            this._cloudStorageService = cloudStorageService;
            this._logger = logger;
        }

        public async Task CreateAsync(VideoCreateDto videoDto, CancellationToken cancellationToken)
        {
            var video = this._mapper.Map(videoDto);
            using (var stream = videoDto.File.OpenReadStream())
            {
                video.Link = await this._cloudStorageService.UploadAsync(stream, videoDto.File.FileName,
                                            videoDto.File.ContentType, "videos", cancellationToken);
            }
            video.Duration = DateTime.MinValue.AddSeconds(videoDto.Duration);

            this._videosRepository.Attach(video);
            await this._videosRepository.AddAsync(video, cancellationToken);

            this._logger.LogInformation($"Created video with id: {video.Id}.");
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            if (await this._videosRepository.ExistsAsync(
                a => a.CoursesMaterials.Any(cm => cm.MaterialId == id), cancellationToken))
            {
                throw new DeleteEntityException("This video is used in other courses!");
            }

            var video = await this._videosRepository.GetOneAsync(id, cancellationToken);
            if (video == null)
            {
                throw new NotFoundException("Video");
            }

            await this._videosRepository.DeleteAsync(video, cancellationToken);

            this._logger.LogInformation($"Deleted video with id: {video.Id}.");
        }

        public async Task<PagedList<VideoDto>> GetPageAsync(PageParameters pageParameters, 
                                                            CancellationToken cancellationToken)
        {
            var videos = await this._videosRepository.GetPageAsync(pageParameters, cancellationToken, v => v.Quality);
            var dtos = this._mapper.Map(videos);

            this._logger.LogInformation($"Returned videos page {videos.PageNumber} from database.");

            return dtos;
        }

        public async Task<IEnumerable<QualityDto>> GetQualitiesAsync(CancellationToken cancellationToken)
        {
            var qualities = await this._qualitiesRepository.GetAllAsync(q => true, cancellationToken);
            var dtos = this._mapper.Map(qualities);

            this._logger.LogInformation($"Returned all qualities from database.");

            return dtos;
        }
    }
}
