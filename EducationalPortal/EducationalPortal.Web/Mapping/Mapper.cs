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

        }).CreateMapper();

        public LearnCourseViewModel Map(Course course)
        {
            var learnCourse = new LearnCourseViewModel
            {
                Id = course.Id,
                Name = course.Name,
                Materials = new List<MaterialsBaseViewModel>(),
            };

            foreach (var material in course.Materials)
            {
                switch (material.GetType().Name)
                {
                    case "Video":
                        var video = (Video)material;
                        learnCourse.Materials.Add(this._mapper.Map<VideoViewModel>(video));
                        break;

                    case "Book":
                        var book = (Book)material;
                        learnCourse.Materials.Add(this._mapper.Map<BookViewModel>(book));
                        break;

                    case "Article":
                        var article = (Article)material;
                        learnCourse.Materials.Add(this._mapper.Map<ArticleViewModel>(article));
                        break;
                        
                    default:
                        break;
                }
            }

            return learnCourse;
        }
    }
}
