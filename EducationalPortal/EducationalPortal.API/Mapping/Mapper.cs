using AutoMapper;
using EducationalPortal.Application.DTO;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.API.ViewModels;
using EducationalPortal.Application.DTO.EducationalMaterials;
using EducationalPortal.Application.DTO.EducationalMaterials.Properties;

namespace EducationalPortal.API.Mapping
{
    public class Mapper
    {
        private readonly IMapper _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Video, VideoViewModel>();

            cfg.CreateMap<Book, BookViewModel>();

            cfg.CreateMap<Article, ArticleViewModel>();

            cfg.CreateMap<Course, CourseViewModel>()
            .ForMember(dest => dest.Materials,
                opt => opt.Ignore());

            cfg.CreateMap<ArticleDTO, Article>();

            cfg.CreateMap<ResourceDTO, Resource>();

            cfg.CreateMap<CourseDTO, Course>()
            .ForMember(dest => dest.Materials,
                opt => opt.Ignore())
            .ForMember(dest => dest.Skills,
                opt => opt.Ignore())
            .ForMember(dest => dest.Id,
                opt => opt.Ignore());

            cfg.CreateMap<Course, CourseDTO>();

            cfg.CreateMap<Skill, SkillDTO>();

            cfg.CreateMap<MaterialsBase, MaterialBaseDTO>();

            cfg.CreateMap<User, UserDTO>();

            cfg.CreateMap<UserDTO, User>();

            cfg.CreateMap<Role, RoleDTO>();

            cfg.CreateMap<BookDTO, Book>();

            cfg.CreateMap<AuthorDTO, Author>();

        }).CreateMapper();

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

        public User Map(User user, UserDTO userDTO)
        {
            return this._mapper.Map(userDTO, user);
        }

        public Article Map(ArticleDTO articleDTO)
        {
            return this._mapper.Map<Article>(articleDTO);
        }

        public Book Map(BookDTO bookDTO)
        {
            return this._mapper.Map<Book>(bookDTO);
        }

        public CourseDTO Map(Course course)
        {
            var courseDTO = this._mapper.Map<CourseDTO>(course);
            return courseDTO;
        }

        public Course Map(CourseDTO courseDTO)
        {
            var course = this._mapper.Map<Course>(courseDTO);
            course = this.MapCourseJoinEntities(course, courseDTO);

            return course;
        }

        public Course Map(Course course, CourseDTO courseDTO)
        {
            course = this._mapper.Map(courseDTO, course);
            course = this.MapCourseJoinEntities(course, courseDTO);
            course.Materials = null;
            course.Skills = null;

            return course;
        }

        private Course MapCourseJoinEntities(Course course, CourseDTO courseDTO)
        {
            course.CoursesMaterials = new List<CoursesMaterials>();
            for (int i = 0; i < courseDTO.Materials.Count; i++)
            {
                var courseMaterial = new CoursesMaterials
                {
                    MaterialId = courseDTO.Materials[i].Id,
                    Index = i + 1,
                };
                course.CoursesMaterials.Add(courseMaterial);
            }

            course.CoursesSkills = new List<CoursesSkills>();
            foreach (var skill in courseDTO.Skills)
            {
                course.CoursesSkills.Add(new CoursesSkills { SkillId = skill.Id });
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
