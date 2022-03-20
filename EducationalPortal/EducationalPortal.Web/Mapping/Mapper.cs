using AutoMapper;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Web.ViewModels;

namespace EducationalPortal.Web.Mapping
{
    public class Mapper
    {
        private readonly IMapper _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Video, VideoViewModel>()
            .ForMember(dest => dest.Quality,
                opt => opt.MapFrom(src => src.Quality.Name));

            cfg.CreateMap<Book, BookViewModel>()
            .ForMember(dest => dest.Extension,
                opt => opt.MapFrom(src => src.Extension.Name));

            cfg.CreateMap<Article, ArticleViewModel>()
            .ForMember(dest => dest.Resource,
                opt => opt.MapFrom(src => src.Resource.Name));

            cfg.CreateMap<Course, CourseViewModel>()
            .ForMember(dest => dest.Materials,
                opt => opt.Ignore());

        }).CreateMapper();

        public VideoViewModel Map(Video source)
        {
            return this._mapper.Map<VideoViewModel>(source);
        }

        public BookViewModel Map(Book source)
        {
            return this._mapper.Map<BookViewModel>(source);
        }

        public ArticleViewModel Map(Article source)
        {
            return this._mapper.Map<ArticleViewModel>(source);
        }

        public CourseViewModel Map(Course source)
        {
            return this._mapper.Map<CourseViewModel>(source);
        }
    }
}
