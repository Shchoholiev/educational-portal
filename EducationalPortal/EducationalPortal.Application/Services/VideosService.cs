using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Repository;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.Application.Services
{
    public class VideosService : IVideosService
    {
        private readonly IGenericRepository<Video> _videosRepository;

        private readonly IGenericRepository<Quality> _qualitiesRepository;

        public VideosService(IGenericRepository<Video> videosRepository, 
                             IGenericRepository<Quality> qualitiesRepository)
        {
            this._videosRepository = videosRepository;
            this._qualitiesRepository = qualitiesRepository;
        }

        public async Task Add(Video video)
        {
            this._videosRepository.Attach(video.Quality);
            await this._videosRepository.Add(video);
        }

        public async Task Delete(int id)
        {
            await this._videosRepository.Delete(id);
        }

        public async Task<IEnumerable<Video>> GetAll()
        {
            return await this._videosRepository.GetAll(v => v.Quality);
        }

        public async Task<Video> GetOne(int id)
        {
            return await this._videosRepository.GetOne(id, v => v.Quality);
        }

        public async Task<IEnumerable<Video>> GetPage(int pageSize, int pageNumber)
        {
            return await this._videosRepository.GetPage(pageSize, pageNumber, v => v.Quality);
        }

        public async Task Update(Video video)
        {
            this._videosRepository.Attach(video.Quality);
            await this._videosRepository.Update(video);
        }

        public async Task<IEnumerable<Quality>> GetQualities()
        {
            return await this._qualitiesRepository.GetAll();
        }
    }
}
