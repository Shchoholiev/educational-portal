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
            cfg.CreateMap<Course, CourseDto>()
               .ForMember(dest => dest.Skills, 
               opt => opt.MapFrom(s => s.CoursesSkills.Select(cs => cs.Skill)));

            cfg.CreateMap<CourseCreateDto, Course>();
            cfg.CreateMap<Course, CourseCreateDto>()
               .ForMember(dest => dest.Skills,
               opt => opt.MapFrom(s => s.CoursesSkills.Select(cs => cs.Skill))); ;

            cfg.CreateMap<Video, VideoDto>();
            cfg.CreateMap<VideoCreateDto, Video>()
               .ForMember(dest => dest.Duration, opt => opt.Ignore());

            cfg.CreateMap<Quality, QualityDto>();
            cfg.CreateMap<QualityDto, Quality>();

            cfg.CreateMap<Book, BookDto>();
            cfg.CreateMap<BookCreateDto, Book>();

            cfg.CreateMap<Extension, ExtensionDto>();

            cfg.CreateMap<Article, ArticleDto>();
            cfg.CreateMap<ArticleCreateDto, Article>();

            cfg.CreateMap<ResourceDto, Resource>();
            cfg.CreateMap<Resource, ResourceDto>();

            cfg.CreateMap<Skill, SkillDto>();
            cfg.CreateMap<SkillDto, Skill>();

            cfg.CreateMap<User, UserDto>();
            cfg.CreateMap<UserDto, User>()
               .ForMember(dest => dest.CreatedCourses, opt => opt.Ignore());
            cfg.CreateMap<UsersSkills, UsersSkillsDto>();

            cfg.CreateMap<Role, RoleDto>();
            cfg.CreateMap<RoleDto, Role>();

            cfg.CreateMap<AuthorDto, Author>();
            cfg.CreateMap<Author, AuthorDto>();

            cfg.CreateMap<CartItem, CartItemDto>();

        }).CreateMapper();

        public PagedList<CourseShortDto> Map(PagedList<Course> source)
        {
            var dtos = this._mapper.Map<PagedList<CourseShortDto>>(source);
            dtos.MapList(source);
            return dtos;
        }

        public CourseDto Map(Course course, IEnumerable<MaterialsBase>? learnedMaterials)
        {
            var courseViewModel = this._mapper.Map<CourseDto>(course);
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

        public PagedList<CartItemDto> Map(PagedList<CartItem> source)
        {
            var dtos = this._mapper.Map<PagedList<CartItemDto>>(source);
            dtos.MapList(source);
            return dtos;
        }

        public User Map(User user, UserDto userDTO)
        {
            return this._mapper.Map(userDTO, user);
        }

        public UserDto Map(User source)
        {
            return this._mapper.Map<UserDto>(source);
        }

        public Article Map(ArticleCreateDto source)
        {
            return this._mapper.Map<Article>(source);
        }

        public PagedList<ArticleDto> Map(PagedList<Article> source)
        {
            var dtos = this._mapper.Map<PagedList<ArticleDto>>(source);
            dtos.MapList(source);
            return dtos;
        }

        public Book Map(BookCreateDto source)
        {
            return this._mapper.Map<Book>(source);
        }

        public PagedList<BookDto> Map(PagedList<Book> source)
        {
            var dtos = this._mapper.Map<PagedList<BookDto>>(source);
            dtos.MapList(source);
            return dtos;
        }

        public Video Map(VideoCreateDto source)
        {
            return this._mapper.Map<Video>(source);
        }

        public PagedList<VideoDto> Map(PagedList<Video> source)
        {
            var dtos = this._mapper.Map<PagedList<VideoDto>>(source);
            dtos.MapList(source);
            return dtos;
        }

        public Resource Map(ResourceDto source)
        {
            return this._mapper.Map<Resource>(source);
        }

        public PagedList<ResourceDto> Map(PagedList<Resource> source)
        {
            var dtos = this._mapper.Map<PagedList<ResourceDto>>(source);
            dtos.MapList(source);
            return dtos;
        }

        public Author Map(AuthorDto source)
        {
            return this._mapper.Map<Author>(source);
        }

        public PagedList<AuthorDto> Map(PagedList<Author> source)
        {
            var dtos = this._mapper.Map<PagedList<AuthorDto>>(source);
            dtos.MapList(source);
            return dtos;
        }

        public Skill Map(SkillDto source)
        {
            return this._mapper.Map<Skill>(source);
        }

        public PagedList<SkillDto> Map(PagedList<Skill> source)
        {
            var dtos = this._mapper.Map<PagedList<SkillDto>>(source);
            dtos.MapList(source);
            return dtos;
        }

        public IEnumerable<QualityDto> Map(IEnumerable<Quality> source)
        {
            return this._mapper.Map<IEnumerable<QualityDto>>(source);
        }

        public CourseCreateDto Map(Course source)
        {
            var courseDTO = this._mapper.Map<CourseCreateDto>(source);
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
            course = this._mapper.Map(courseDTO, course);
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

            course.CoursesSkills = new List<CoursesSkills>();
            foreach (var skill in courseDTO.Skills)
            {
                course.CoursesSkills.Add(new CoursesSkills { SkillId = skill.Id });
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
                        var videoViewModel = this._mapper.Map<VideoDto>(video);
                        videoViewModel.IsLearned = learnedMaterials?.Any(m => m.Id == material.Id) ?? false;
                        materialsViewModel.Add(videoViewModel);
                        break;

                    case "Book":
                        var book = (Book)material;
                        var bookViewModel = this._mapper.Map<BookDto>(book);
                        bookViewModel.IsLearned = learnedMaterials?.Any(m => m.Id == material.Id) ?? false;
                        materialsViewModel.Add(bookViewModel);
                        break;

                    case "Article":
                        var article = (Article)material;
                        var articleViewModel = this._mapper.Map<ArticleDto>(article);
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
