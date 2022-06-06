using AutoMapper;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.EducationalMaterials.Properties;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Models.DTO.EducationalMaterials;
using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO.Course;
using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Mapping
{
    public class Mapper
    {
        private readonly IMapper _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Course, CourseShortDto>();

            cfg.CreateMap<Course, CourseDto>();

            cfg.CreateMap<Video, VideoDto>();

            cfg.CreateMap<Book, BookDto>();

            cfg.CreateMap<Article, ArticleDto>();

            cfg.CreateMap<ArticleCreateDto, Article>();

            cfg.CreateMap<ResourceDto, Resource>();

            cfg.CreateMap<CourseCreateDto, Course>()
            .ForMember(dest => dest.Skills,
                opt => opt.Ignore());

            cfg.CreateMap<Course, CourseCreateDto>();

            cfg.CreateMap<Skill, SkillDto>();

            cfg.CreateMap<MaterialsBase, MaterialBaseDto>();

            cfg.CreateMap<User, UserDto>();

            cfg.CreateMap<UserDto, User>();

            cfg.CreateMap<Role, RoleDto>();

            cfg.CreateMap<BookCreateDto, Book>();

            cfg.CreateMap<AuthorDto, Author>();

        }).CreateMapper();

        public PagedList<CourseShortDto> Map(PagedList<Course> courses)
        {
            return this._mapper.Map<PagedList<CourseShortDto>>(courses);
        }

        public CourseDto Map(Course course, IEnumerable<MaterialsBase>? learnedMaterials)
        {
            var courseViewModel = _mapper.Map<CourseDto>(course);
            courseViewModel.Materials = MapMaterials(course.CoursesMaterials.Select(cm => cm.Material), 
                                                     learnedMaterials);

            return courseViewModel;
        }

        public CourseLearnDto MapLearnCourse(Course course, IEnumerable<MaterialsBase>? learnedMaterials)
        {
            var learnCourse = new CourseLearnDto
            {
                Id = course.Id,
                Name = course.Name,
            };
            learnCourse.Materials = MapMaterials(course.CoursesMaterials.Select(cm => cm.Material), 
                                                 learnedMaterials);

            return learnCourse;
        }

        public User Map(User user, UserDto userDTO)
        {
            return _mapper.Map(userDTO, user);
        }

        public Article Map(ArticleCreateDto articleDTO)
        {
            return _mapper.Map<Article>(articleDTO);
        }

        public Book Map(BookCreateDto bookDTO)
        {
            return _mapper.Map<Book>(bookDTO);
        }

        public CourseCreateDto Map(Course course)
        {
            var courseDTO = _mapper.Map<CourseCreateDto>(course);
            return courseDTO;
        }

        public Course Map(CourseCreateDto courseDTO)
        {
            var course = _mapper.Map<Course>(courseDTO);
            course = this.MapCourseJoinEntities(course, courseDTO);

            return course;
        }

        public Course Map(Course course, CourseCreateDto courseDTO)
        {
            course = _mapper.Map(courseDTO, course);
            course = MapCourseJoinEntities(course, courseDTO);

            return course;
        }

        private Course MapCourseJoinEntities(Course course, CourseCreateDto courseDTO)
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

            return course;
        }

        private List<MaterialBaseDto> MapMaterials(IEnumerable<MaterialsBase> materials,
                                                   IEnumerable<MaterialsBase>? learnedMaterials)
        {
            var materialsViewModel = new List<MaterialBaseDto>();
            foreach (var material in materials)
            {
                switch (material.GetType().Name)
                {
                    case "Video":
                        var video = (Video)material;
                        var videoViewModel = _mapper.Map<VideoDto>(video);
                        videoViewModel.IsLearned = learnedMaterials?.Any(m => m.Id == material.Id) ?? false;
                        materialsViewModel.Add(videoViewModel);
                        break;

                    case "Book":
                        var book = (Book)material;
                        var bookViewModel = _mapper.Map<BookDto>(book);
                        bookViewModel.IsLearned = learnedMaterials?.Any(m => m.Id == material.Id) ?? false;
                        materialsViewModel.Add(bookViewModel);
                        break;

                    case "Article":
                        var article = (Article)material;
                        var articleViewModel = _mapper.Map<ArticleDto>(article);
                        articleViewModel.IsLearned = learnedMaterials?.Any(m => m.Id == material.Id) ?? false;
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
