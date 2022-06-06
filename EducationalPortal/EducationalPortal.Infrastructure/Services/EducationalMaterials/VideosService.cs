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

namespace EducationalPortal.Infrastructure.Services.EducationalMaterials
{
    public class VideosService : IVideosService
    {
        private readonly IGenericRepository<Video> _videosRepository;

        private readonly IGenericRepository<Quality> _qualitiesRepository;

        private readonly ICloudStorageService _cloudStorageService;

        private readonly Mapper _mapper = new();

        public VideosService(IGenericRepository<Video> videosRepository,
                                IGenericRepository<Quality> qualitiesRepository,
                                ICloudStorageService cloudStorageService)
        {
            this._videosRepository = videosRepository;
            this._qualitiesRepository = qualitiesRepository;
            this._cloudStorageService = cloudStorageService;
        }

        public async Task CreateAsync(VideoCreateDto videoDto)
        {
            var video = this._mapper.Map(videoDto);
            using (var stream = videoDto.File.OpenReadStream())
            {
                video.Link = await this._cloudStorageService.UploadAsync(stream, videoDto.File.FileName,
                                                                    videoDto.File.ContentType, "videos");
            }
            video.Duration = DateTime.MinValue.AddSeconds(videoDto.Duration);

            this._videosRepository.Attach(video);
            await this._videosRepository.AddAsync(video);
        }

        public async Task DeleteAsync(int id)
        {
            if (await this._videosRepository.Exists(a => a.CoursesMaterials.Any(cm => cm.MaterialId == id)))
            {
                throw new DeleteEntityException("This video is used in other courses!");
            }

            var video = await this._videosRepository.GetOneAsync(id);
            if (video == null)
            {
                throw new NotFoundException("Video");
            }

            await this._videosRepository.DeleteAsync(video);
        }

        public async Task<PagedList<VideoDto>> GetPageAsync(PageParameters pageParameters)
        {
            var videos = await this._videosRepository.GetPageAsync(pageParameters);
            var dtos = this._mapper.Map(videos);
            return dtos;
        }

        public async Task<IEnumerable<QualityDto>> GetQualitiesAsync()
        {
            var qualities = await this._qualitiesRepository.GetAllAsync(q => true);
            var dtos = this._mapper.Map(qualities);
            return dtos;
        }
    }
}
