using AutoMapper;
using EducationalPortal.Application.DTO;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Web.ViewModels;
using EducationalPortal.Web.ViewModels.CreateViewModels;

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

            cfg.CreateMap<Skill, SkillCreateModel>();

            cfg.CreateMap<Video, VideoCreateModel>();

            cfg.CreateMap<Book, BookCreateModel>();

            cfg.CreateMap<Author, AuthorCreateModel>();

            cfg.CreateMap<Article, ArticleCreateModel>();

            cfg.CreateMap<ArticleDTO, Article>();

            cfg.CreateMap<Resource, ResourceCreateModel>();

            cfg.CreateMap<CourseDTO, Course>()
            .ForMember(dest => dest.Materials,
                opt => opt.Ignore())
            .ForMember(dest => dest.Skills,
                opt => opt.Ignore());

            cfg.CreateMap<Course, CourseDTO>();

        }).CreateMapper();

        public IEnumerable<SkillCreateModel> Map(IEnumerable<Skill> skills, IEnumerable<Skill> chosenSkills)
        {
            var skillsCreateModels = this._mapper.Map<IEnumerable<SkillCreateModel>>(skills);
            foreach (var skill in chosenSkills)
            {
                if (skillsCreateModels.Any(s => s.Id == skill.Id))
                {
                    skillsCreateModels.FirstOrDefault(s => s.Id == skill.Id).IsChosen = true;
                }
            }

            return skillsCreateModels;
        }

        public IEnumerable<AuthorCreateModel> Map(IEnumerable<Author> authors, IEnumerable<Author> chosenAuthors)
        {
            var authorCreateModels = this._mapper.Map<IEnumerable<AuthorCreateModel>>(authors);
            foreach (var author in chosenAuthors)
            {
                if (authorCreateModels.Any(s => s.Id == author.Id))
                {
                    authorCreateModels.FirstOrDefault(a => a.Id == author.Id).IsChosen = true;
                }
            }

            return authorCreateModels;
        }

        public IEnumerable<VideoCreateModel> Map(IEnumerable<Video> videos, IEnumerable<Video> chosenVideos)
        {
            var videosCreateModels = this._mapper.Map<IEnumerable<VideoCreateModel>>(videos);
            foreach (var video in chosenVideos)
            {
                if (videosCreateModels.Any(v => v.Id == video.Id))
                {
                    videosCreateModels.FirstOrDefault(v => v.Id == video.Id).IsChosen = true;
                }
            }

            return videosCreateModels;
        }

        public IEnumerable<ArticleCreateModel> Map(IEnumerable<Article> articles, IEnumerable<Article> chosenArticles)
        {
            var articlesCreateModels = this._mapper.Map<IEnumerable<ArticleCreateModel>>(articles);
            foreach (var article in chosenArticles)
            {
                if (articlesCreateModels.Any(a => a.Id == article.Id))
                {
                    articlesCreateModels.FirstOrDefault(v => v.Id == article.Id).IsChosen = true;
                }
            }

            return articlesCreateModels;
        }

        public IEnumerable<ResourceCreateModel> Map(IEnumerable<Resource> resources, Resource chosenResource)
        {
            var resourcesCreateModels = this._mapper.Map<IEnumerable<ResourceCreateModel>>(resources);
            if (resources.Any(r => r.Id == chosenResource.Id))
            {
                resourcesCreateModels.FirstOrDefault(r => r.Id == chosenResource.Id).IsChosen = true;
            }

            return resourcesCreateModels;
        }

        public IEnumerable<BookCreateModel> Map(IEnumerable<Book> books, IEnumerable<Book> chosenBooks)
        {
            var booksCreateModels = this._mapper.Map<IEnumerable<BookCreateModel>>(books);
            foreach (var book in chosenBooks)
            {
                if (booksCreateModels.Any(b => b.Id == book.Id))
                {
                    booksCreateModels.FirstOrDefault(b => b.Id == book.Id).IsChosen = true;
                }
            }

            return booksCreateModels;
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

        public Article Map(ArticleDTO articleDTO)
        {
            return this._mapper.Map<Article>(articleDTO);
        }

        public CourseDTO Map(Course course)
        {
            var courseDTO = this._mapper.Map<CourseDTO>(course);
            return courseDTO;
        }

        public Course Map(CourseDTO courseDTO)
        {
            var course = this._mapper.Map<Course>(courseDTO);
            
            course.CoursesMaterials = new List<CoursesMaterials>();
            for (int i = 0; i < courseDTO.Materials.Count; i++)
            {
                var courseMaterial = new CoursesMaterials
                {
                    Material = courseDTO.Materials[i],
                    Index = i + 1,
                };
                course.CoursesMaterials.Add(courseMaterial);
            }

            course.CoursesSkills = new List<CoursesSkills>();
            foreach (var skill in courseDTO.Skills)
            {
                course.CoursesSkills.Add(new CoursesSkills { Skill = skill });
            }

            return course;
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
