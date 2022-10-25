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
using EducationalPortal.Application.Models.QueryModels;
using EducationalPortal.Core.Entities.FinalTasks;
using EducationalPortal.Application.Models.DTO.FinalTasks;
using EducationalPortal.Application.Models.LookupModels;
using EducationalPortal.Application.Models.StatisticsModel;
using EducationalPortal.Application.Models.QueryModels.Statistics;

namespace EducationalPortal.Application.Mapping
{
    public class Mapper
    {
        private readonly IMapper _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<MaterialsBase, MaterialBaseDto>();
            cfg.CreateMap<MaterialsBase, MaterialBaseCreateDto>();
            cfg.CreateMap<MaterialQueryModel, MaterialBaseDto>();

            cfg.CreateMap<Course, CourseShortDto>();
            cfg.CreateMap<CourseShortQueryModel, CourseShortDto>();
            cfg.CreateMap<CourseQueryModel, CourseDto>()
               .ForMember(dest => dest.Materials, opt => opt.Ignore());
            cfg.CreateMap<CourseQueryModel, CourseCreateDto>()
               .ForMember(dest => dest.Materials, opt => opt.Ignore());
            cfg.CreateMap<Course, CourseDto>()
               .ForMember(dest => dest.Skills, 
               opt => opt.MapFrom(s => s.CoursesSkills.Select(cs => cs.Skill)));

            cfg.CreateMap<CourseCreateDto, Course>();
            cfg.CreateMap<Course, CourseCreateDto>()
               .ForMember(dest => dest.Skills,
               opt => opt.MapFrom(s => s.CoursesSkills.Select(cs => cs.Skill))); ;

            cfg.CreateMap<MaterialQueryModel, VideoDto>();
            cfg.CreateMap<Video, VideoDto>();
            cfg.CreateMap<VideoCreateDto, Video>()
               .ForMember(dest => dest.Duration, opt => opt.Ignore());

            cfg.CreateMap<Quality, QualityDto>();
            cfg.CreateMap<QualityDto, Quality>();

            cfg.CreateMap<MaterialQueryModel, BookDto>();
            cfg.CreateMap<Book, BookDto>();
            cfg.CreateMap<BookCreateDto, Book>();

            cfg.CreateMap<Extension, ExtensionDto>();

            cfg.CreateMap<MaterialQueryModel, ArticleDto>();
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

            cfg.CreateMap<FinalTask, FinalTaskDto>();
            cfg.CreateMap<FinalTaskDto, FinalTask>();

            cfg.CreateMap<ReviewQuestion, ReviewQuestionDto>();
            cfg.CreateMap<ReviewQuestionDto, ReviewQuestion>();

            cfg.CreateMap<SubmittedReviewQuestionDto, SubmittedReviewQuestion>();

            cfg.CreateMap<SubmittedFinalTaskDto, SubmittedFinalTask>();
            cfg.CreateMap<SubmittedFinalTask, SubmittedFinalTaskDto>();

            cfg.CreateMap<MaterialStatisticsQueryModel, MaterialStatisticsModel>();
            cfg.CreateMap<SalesStatisticsQueryModel, SalesStatisticsModel>();
            cfg.CreateMap<UserStatisticsQueryModel, UserStatisticsModel>();
            cfg.CreateMap<CourseShortQueryModel, CourseStatisticsModel>();

        }).CreateMapper();

        public PagedList<CourseShortDto> Map(PagedList<Course> source)
        {
            var dtos = this._mapper.Map<PagedList<CourseShortDto>>(source);
            dtos.MapList(source);
            return dtos;
        }

        public PagedList<CourseShortDto> Map(PagedList<CourseShortQueryModel> source)
        {
            var dtos = this._mapper.Map<PagedList<CourseShortDto>>(source);
            dtos.MapList(source);
            return dtos;
        }

        public List<CourseShortDto> Map(List<CourseShortQueryModel> source)
        {
            return this._mapper.Map<List<CourseShortDto>>(source);
        }

        public CourseDto Map(CourseQueryModel course)
        {
            var courseViewModel = this._mapper.Map<CourseDto>(course);
            courseViewModel.Materials = MapMaterials(course.Materials);

            return courseViewModel;
        }

        public CourseCreateDto MapForEdit(CourseQueryModel course)
        {
            var dto = this._mapper.Map<CourseCreateDto>(course);
            dto.Materials = MapMaterials(course.Materials);

            return dto;
        }

        public CourseLearnDto MapLearnCourse(CourseQueryModel course)
        {
            var learnCourse = new CourseLearnDto
            {
                Id = course.Id,
                Name = course.Name,
                Materials = MapMaterials(course.Materials),
            };

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

        public Course Map(CourseCreateDto courseDTO)
        {
            var course = _mapper.Map<Course>(courseDTO);
            course = this.MapCourseJoinEntities(course, courseDTO);

            return course;
        }

        public PagedList<FinalTaskDto> Map(PagedList<FinalTask> source)
        {
            var dtos = this._mapper.Map<PagedList<FinalTaskDto>>(source);
            dtos.MapList(source);
            return dtos;
        }

        public FinalTask Map(FinalTaskDto source)
        {
            return this._mapper.Map<FinalTask>(source);
        }

        public SubmittedFinalTask Map(SubmittedFinalTaskDto source)
        {
            return this._mapper.Map<SubmittedFinalTask>(source);
        }

        public IEnumerable<SubmittedReviewQuestion> Map(IEnumerable<SubmittedReviewQuestionDto> source, int submittedFinalTaskId)
        {
            var submittedQuestions = this._mapper.Map<IEnumerable<SubmittedReviewQuestion>>(source);
            foreach (var question in submittedQuestions)
            {
                question.SubmittedFinalTaskId = submittedFinalTaskId;
            }

            return submittedQuestions;
        }

        public IEnumerable<ReviewQuestionDto> Map(IEnumerable<ReviewQuestion> source)
        {
            return this._mapper.Map<IEnumerable<ReviewQuestionDto>>(source);
        }

        public FinalTaskDto Map(FinalTask source)
        {
            return this._mapper.Map<FinalTaskDto>(source);
        }

        public Course Map(Course course, CourseCreateDto courseDTO)
        {
            course = this._mapper.Map(courseDTO, course);
            course = MapCourseJoinEntities(course, courseDTO);

            return course;
        }

        public SubmittedFinalTaskDto Map(SubmittedFinalTask source)
        {
            return this._mapper.Map<SubmittedFinalTaskDto>(source);
        }

        public List<SkillDto> Map(IEnumerable<Skill> skills, IEnumerable<SkillLookupModel> skillLookups)
        {
            var dtos = new List<SkillDto>();
            foreach (var skill in skills)
            {
                dtos.Add(new SkillDto
                {
                    Id = skill.Id,
                    Name = skill.Name,
                    Level = skillLookups.FirstOrDefault(s => s.SkillId == skill.Id).Level,
                });
            }

            return dtos;
        }

        public PagedList<MaterialStatisticsModel> Map(PagedList<MaterialStatisticsQueryModel> source)
        {
            var dtos = this._mapper.Map<PagedList<MaterialStatisticsModel>>(source);
            dtos.MapList(source);
            return dtos;
        }

        public PagedList<UserStatisticsModel> Map(PagedList<UserStatisticsQueryModel> source)
        {
            var dtos = this._mapper.Map<PagedList<UserStatisticsModel>>(source);
            dtos.MapList(source);
            foreach (var user in dtos)
            {
                user.CompletedCoursesPercentage = user.CompletedCoursesCount * 100 / user.BoughtCoursesCount;
            }

            return dtos;
        }

        public PagedList<CourseStatisticsModel> MapStatistics(PagedList<CourseShortQueryModel> source)
        {
            var dtos = this._mapper.Map<PagedList<CourseStatisticsModel>>(source);
            dtos.MapList(source);
            return dtos;
        }

        public SalesStatisticsModel Map(SalesStatisticsQueryModel source)
        {
            return this._mapper.Map<SalesStatisticsModel>(source);
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

            course.CoursesSkills = courseDTO.Skills.Select(s => new CoursesSkills { SkillId = s.Id }).ToList();

            return course;
        }

        private List<MaterialBaseDto> MapMaterials(IEnumerable<MaterialQueryModel> materials)
        {
            var materialsViewModel = new List<MaterialBaseDto>();
            foreach (var material in materials)
            {
                if (material.Extension != null)
                {
                    var videoViewModel = this._mapper.Map<BookDto>(material);
                    materialsViewModel.Add(videoViewModel);
                }
                else if (material.Resource != null)
                {
                    var bookViewModel = this._mapper.Map<ArticleDto>(material);
                    materialsViewModel.Add(bookViewModel);
                }
                else
                {
                    var articleViewModel = this._mapper.Map<VideoDto>(material);
                    materialsViewModel.Add(articleViewModel);
                }
            }

            return materialsViewModel;
        }
    }
}
