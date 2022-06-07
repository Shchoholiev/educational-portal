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
            cfg.CreateMap<MaterialsBase, MaterialBaseDto>();
            cfg.CreateMap<MaterialsBase, MaterialBaseCreateDto>();

            cfg.CreateMap<Course, CourseShortDto>();
            cfg.CreateMap<Course, CourseDto>();
            cfg.CreateMap<CourseCreateDto, Course>();
            cfg.CreateMap<Course, CourseCreateDto>();

            cfg.CreateMap<Video, VideoDto>();
            cfg.CreateMap<VideoCreateDto, Video>()
               .ForMember(dest => dest.Duration, opt => opt.Ignore());

            cfg.CreateMap<Quality, QualityDto>();
            cfg.CreateMap<QualityDto, Quality>();

            cfg.CreateMap<Book, BookDto>();
            cfg.CreateMap<BookCreateDto, Book>();

            cfg.CreateMap<Article, ArticleDto>();
            cfg.CreateMap<ArticleCreateDto, Article>();

            cfg.CreateMap<ResourceDto, Resource>();
            cfg.CreateMap<Resource, ResourceDto>();

            cfg.CreateMap<Skill, SkillDto>();
            cfg.CreateMap<SkillDto, Skill>();

            cfg.CreateMap<User, UserDto>();
            cfg.CreateMap<UserDto, User>();

            cfg.CreateMap<Role, RoleDto>();
            cfg.CreateMap<RoleDto, Role>();

            cfg.CreateMap<AuthorDto, Author>();
            cfg.CreateMap<Author, AuthorDto>();

            cfg.CreateMap<CartItem, CartItemDto>();

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

        public PagedList<CartItemDto> Map(PagedList<CartItem> courses)
        {
            return this._mapper.Map<PagedList<CartItemDto>>(courses);
        }

        public User Map(User user, UserDto userDTO)
        {
            return _mapper.Map(userDTO, user);
        }

        public UserDto Map(User source)
        {
            return _mapper.Map<UserDto>(source);
        }

        public Article Map(ArticleCreateDto source)
        {
            return _mapper.Map<Article>(source);
        }

        public PagedList<ArticleDto> Map(PagedList<Article> source)
        {
            return _mapper.Map<PagedList<ArticleDto>>(source);
        }

        public Book Map(BookCreateDto source)
        {
            return _mapper.Map<Book>(source);
        }

        public PagedList<BookDto> Map(PagedList<Book> source)
        {
            return _mapper.Map<PagedList<BookDto>>(source);
        }

        public Video Map(VideoCreateDto source)
        {
            return _mapper.Map<Video>(source);
        }

        public PagedList<VideoDto> Map(PagedList<Video> source)
        {
            return _mapper.Map<PagedList<VideoDto>>(source);
        }

        public Resource Map(ResourceDto source)
        {
            return _mapper.Map<Resource>(source);
        }

        public PagedList<ResourceDto> Map(PagedList<Resource> source)
        {
            return _mapper.Map<PagedList<ResourceDto>>(source);
        }

        public Author Map(AuthorDto source)
        {
            return _mapper.Map<Author>(source);
        }

        public PagedList<AuthorDto> Map(PagedList<Author> source)
        {
            return _mapper.Map<PagedList<AuthorDto>>(source);
        }

        public Skill Map(SkillDto source)
        {
            return _mapper.Map<Skill>(source);
        }

        public PagedList<SkillDto> Map(PagedList<Skill> source)
        {
            return this._mapper.Map<PagedList<SkillDto>>(source);
        }

        public IEnumerable<QualityDto> Map(IEnumerable<Quality> source)
        {
            return _mapper.Map<IEnumerable<QualityDto>>(source);
        }

        public CourseCreateDto Map(Course source)
        {
            var courseDTO = _mapper.Map<CourseCreateDto>(source);
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
