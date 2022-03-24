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

            cfg.CreateMap<Skill, SkillViewModel>();

        }).CreateMapper();

        public IEnumerable<SkillViewModel> Map(IEnumerable<Skill> skills, IEnumerable<Skill> chosenSkills)
        {
            var skillsViewModels = this._mapper.Map<IEnumerable<SkillViewModel>>(skills);
            foreach (var skill in skillsViewModels)
            {
                if (chosenSkills.Any(s => s.Id == skill.Id))
                {
                    skillsViewModels.FirstOrDefault(s => s.Id == skill.Id).IsChosen = true;
                }
            }

            return skillsViewModels;
        }

        public CourseViewModel Map(Course course, List<MaterialsBase> learnedMaterials)
        {
            var courseViewModel = this._mapper.Map<CourseViewModel>(course);
            courseViewModel.Materials = this.MapMaterials(course.Materials, learnedMaterials);
            return courseViewModel;
        }

        public LearnCourseViewModel MapLearnCourse(Course course, List<MaterialsBase> learnedMaterials)
        {
            var learnCourse = new LearnCourseViewModel
            {
                Id = course.Id,
                Name = course.Name,
            };
            learnCourse.Materials = this.MapMaterials(course.Materials, learnedMaterials);

            return learnCourse;
        }

        private List<MaterialsBaseViewModel> MapMaterials(List<MaterialsBase> materials, 
                                                         List<MaterialsBase> learnedMaterials)
        {
            var materialsViewModel = new List<MaterialsBaseViewModel>();
            foreach (var material in materials)
            {
                switch (material.GetType().Name)
                {
                    case "Video":
                        var video = (Video)material;
                        var videoViewModel = this._mapper.Map<VideoViewModel>(video);
                        videoViewModel.IsLearned = learnedMaterials.Any(m => m.Id == material.Id);
                        materialsViewModel.Add(videoViewModel);
                        break;

                    case "Book":
                        var book = (Book)material;
                        var bookViewModel = this._mapper.Map<BookViewModel>(book);
                        bookViewModel.IsLearned = learnedMaterials.Any(m => m.Id == material.Id);
                        materialsViewModel.Add(bookViewModel);
                        break;

                    case "Article":
                        var article = (Article)material;
                        var articleViewModel = this._mapper.Map<ArticleViewModel>(article);
                        articleViewModel.IsLearned = learnedMaterials.Any(m => m.Id == material.Id);
                        materialsViewModel.Add(articleViewModel);
                        break;

                    default:
                        break;
                }
            }

            return materialsViewModel;
        }
    }
}
