using EducationalPortal.Application.Exceptions;
using EducationalPortal.Application.Interfaces;
using EducationalPortal.Application.Interfaces.Repositories;
using EducationalPortal.Application.Mapping;
using EducationalPortal.Application.Models.CreateDTO;
using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Models.DTO.Course;
using EducationalPortal.Application.Models.LookupModels;
using EducationalPortal.Application.Paging;
using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.JoinEntities;
using EducationalPortal.Core.Enums;
using EducationalPortal.Infrastructure.CustomMiddlewares;
using iText.IO.Font.Constants;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using Microsoft.Extensions.Logging;

namespace EducationalPortal.Infrastructure.Services
{
    public class CoursesService : ICoursesService
    {
        private readonly ICoursesRepository _coursesRepository;

        private readonly IUsersRepository _usersRepository;

        private readonly IUsersCoursesRepository _usersCoursesRepository;

        private readonly IGenericRepository<MaterialsBase> _materialsRepository;

        private readonly ICertificatesService _certificatesService;

        private readonly ILogger _logger;

        private readonly Mapper _mapper = new();

        private const string _searchResultsPdfUrl = "https://educationalportal.blob.core.windows.net/essentials/Search%20results.pdf";

        private const string _robotoSlabBold = "https://educationalportal.blob.core.windows.net/essentials/RobotoSlab-Bold.ttf";

        public CoursesService(ICoursesRepository coursesRepository, IUsersRepository usersRepository,
                              IUsersCoursesRepository usersCoursesRepository,
                              IGenericRepository<MaterialsBase> materialsRepository,
                              ICertificatesService certificatesService,
                              ILogger<CoursesService> logger)
        {
            this._coursesRepository = coursesRepository;
            this._usersRepository = usersRepository;
            this._usersCoursesRepository = usersCoursesRepository;
            this._materialsRepository = materialsRepository;
            this._certificatesService = certificatesService;
            this._logger = logger;
        }

        public async Task<Course> CreateAsync(CourseCreateDto courseDto, string authorEmail, CancellationToken cancellationToken)
        {
            var course = this._mapper.Map(courseDto);
            var author = await this._usersRepository.GetUserAsync(authorEmail, cancellationToken);
            course.Author = author;
            course.UpdateDateUTC = DateTime.UtcNow;

            await this._coursesRepository.AddAsync(course, cancellationToken);

            this._logger.LogInformation($"Created course with id: {course.Id}.");

            return course;
        }

        public async Task UpdateAsync(int id, CourseCreateDto courseDto, CancellationToken cancellationToken)
        {
            var course = await this._coursesRepository.GetCourseAsync(id, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException("Course");
            }

            this._mapper.Map(course, courseDto);
            course.UpdateDateUTC = DateTime.UtcNow;

            await this._coursesRepository.UpdateAsync(course, cancellationToken);

            this._logger.LogInformation($"Updated course with id: {course.Id}.");
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var course = await this._coursesRepository.GetCourseAsync(id, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException("Course");
            }
            await this._coursesRepository.DeleteAsync(course, cancellationToken);

            this._logger.LogInformation($"Deleted course with id: {course.Id}.");
        }

        public async Task<CourseDto> GetCourseAsync(int id, string? userId, CancellationToken cancellationToken)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id, userId ?? string.Empty, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException("Course");
            }

            var dto = this._mapper.Map(course);

            this._logger.LogInformation($"Returned course with id: {course.Id}.");

            return dto;
        }

        public async Task<CourseCreateDto> GetCourseForEditAsync(int id, CancellationToken cancellationToken)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id, string.Empty, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException("Course");
            }

            var courseDTO = this._mapper.MapForEdit(course);

            this._logger.LogInformation($"Returned course for edit with id: {course.Id}.");

            return courseDTO;
        }

        public async Task<CourseLearnDto> GetCourseLearnAsync(int id, string userId, CancellationToken cancellationToken)
        {
            var course = await this._coursesRepository.GetFullCourseAsync(id, userId, cancellationToken);
            if (course == null)
            {
                throw new NotFoundException("Course");
            }

            var dto = this._mapper.MapLearnCourse(course);
            var userCourse = await this._usersCoursesRepository.GetUsersCoursesAsync(course.Id, userId, cancellationToken);
            dto.Progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);

            this._logger.LogInformation($"Returned course learn with id: {course.Id}.");

            return dto;
        }

        public async Task<PagedList<CourseShortDto>> GetPageAsync(PageParameters pageParameters, CancellationToken cancellationToken)
        {
            var courses = await this._coursesRepository.GetPageAsync(pageParameters, cancellationToken);
            var coursesDtos = this._mapper.Map(courses);

            this._logger.LogInformation($"Returned courses page {courses.PageNumber} from database.");

            return coursesDtos;
        }

        public async Task<PagedList<CourseShortDto>> GetFilteredPageAsync(PageParameters pageParameters, 
            string filter, CoursesOrderBy orderBy, bool isAscending, CancellationToken cancellationToken)
        {
            var courses = await this._coursesRepository.GetPageAsync(pageParameters, filter, 
                orderBy, isAscending, cancellationToken);
            var coursesDtos = this._mapper.Map(courses);

            this._logger.LogInformation($"Returned courses page {courses.PageNumber} from database.");

            return coursesDtos;
        }

        public async Task<int> MaterialLearnedAsync(int materialId, int courseId, string userId, CancellationToken cancellationToken)
        {
            var userCourse = await this._usersCoursesRepository.GetUsersCoursesAsync(courseId, userId, cancellationToken);
            if (userCourse == null)
            {
                throw new NotFoundException("User and Course");
            }

            userCourse.LearnedMaterialsCount++;
            await this._usersCoursesRepository.UpdateUsersCoursesAsync(userCourse, cancellationToken);

            var user = await this._usersRepository.GetUserWithMaterialsAsync(userId, cancellationToken);
            user.Materials.Add(await this._materialsRepository.GetOneAsync(materialId, cancellationToken));
            await this._usersRepository.UpdateAsync(user, cancellationToken);

            this._logger.LogInformation($"Material with id: {materialId} added to user with id: {userId}." +
                                        $" Material learned.");

            var progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);
            if (progress == 100 && !await _certificatesService.ExistsAsync(courseId, userId, cancellationToken))
            {
                await this._usersRepository.AddAcquiredSkillsAsync(courseId, userId, cancellationToken);
                await this._certificatesService.CreateAsync(courseId, userId, cancellationToken);
                this._logger.LogInformation($"Added skills of course with id: {courseId} " +
                                            $"to user with id: {userId}. Course learned.");
            }

            return progress;
        }

        public async Task<int> MaterialUnearnedAsync(int materialId, int courseId, string userId, CancellationToken cancellationToken)
        {
            var userCourse = await this._usersCoursesRepository.GetUsersCoursesAsync(courseId, userId, cancellationToken);
            if (userCourse == null)
            {
                throw new NotFoundException("UserCourse");
            }

            userCourse.LearnedMaterialsCount--;
            await this._usersCoursesRepository.UpdateUsersCoursesAsync(userCourse, cancellationToken);

            var user = await this._usersRepository.GetUserWithMaterialsAsync(userId, cancellationToken);
            user.Materials.Remove(user.Materials.FirstOrDefault(m => m.Id == materialId));
            await this._usersRepository.UpdateAsync(user, cancellationToken);

            this._logger.LogInformation($"Material with id: {materialId} added removed from user with " +
                                        $"id: {userId}. Material unlearned.");

            var progress = (int)(userCourse.LearnedMaterialsCount * 100 / userCourse.MaterialsCount);
            return progress;
        }

        public async Task<List<CourseShortDto>> GetCoursesByAutomatedSearchAsync(
            List<SkillLookupModel> skillLookups, string userId, CancellationToken cancellationToken)
        {
            var chosenCoursesIds = new List<int>();
            while (skillLookups.Count > 0)
            {
                var unprocessed = await _coursesRepository.GetLookupModelsAsync(
                    skillLookups.Select(sl => sl.SkillId).ToList(), chosenCoursesIds, userId, cancellationToken);

                if (unprocessed.Count == 0)
                {
                    throw new NoResultException("There is no courses to fulfill the requirements");
                }

                var courseLookups = this.ProcessCourseLookups(unprocessed, skillLookups);

                var chosenCourse = courseLookups.OrderByDescending(cl => cl.Levels).FirstOrDefault();
                foreach (var cs in chosenCourse.CoursesSkills)
                {
                    var skillLookup = skillLookups.FirstOrDefault(sl => sl.SkillId == cs.SkillId);
                    skillLookup.Level -= cs.Level;
                    if (skillLookup.Level <= 0)
                    {
                        skillLookups.Remove(skillLookup);
                    }
                }
                chosenCoursesIds.Add(chosenCourse.CourseId);
            }

            var courses = await this._coursesRepository.GetCoursesAsync(chosenCoursesIds, cancellationToken);
            var coursesDtos = this._mapper.Map(courses);

            this._logger.LogInformation($"Returned courses by automated search from database.");

            return coursesDtos;
        }

        public async Task<List<CourseShortDto>> GetCoursesByAutomatedSearchBasedOnTimeAsync(
            List<SkillLookupModel> skillLookups, string userId, CancellationToken cancellationToken)
        {
            var chosenCoursesIds = new List<int>();
            var materialIds = new List<int>();
            while (skillLookups.Count > 0)
            {
                var unprocessed = await _coursesRepository.GetLookupModelsAsync(skillLookups.Select(sl => sl.SkillId).ToList(), 
                    chosenCoursesIds, materialIds, userId, cancellationToken);

                if (unprocessed.Count == 0)
                {
                    throw new NoResultException("There is no courses to fulfill the requirements");
                }

                var courseLookups = this.ProcessCourseLookups(unprocessed, skillLookups);

                var chosenCourse = courseLookups.OrderByDescending(cl => cl.Levels)
                    .ThenBy(cl => cl.LearningTime)
                    .FirstOrDefault();
                foreach (var cs in chosenCourse.CoursesSkills)
                {
                    var skillLookup = skillLookups.FirstOrDefault(sl => sl.SkillId == cs.SkillId);
                    skillLookup.Level -= cs.Level;
                    if (skillLookup.Level <= 0)
                    {
                        skillLookups.Remove(skillLookup);
                    }

                    materialIds.AddRange(chosenCourse.MaterialIds);
                }
                chosenCoursesIds.Add(chosenCourse.CourseId);
            }

            var courses = await this._coursesRepository.GetCoursesAsync(chosenCoursesIds, cancellationToken);
            var coursesDtos = this._mapper.Map(courses);

            this._logger.LogInformation($"Returned courses by automated search from database.");

            return coursesDtos;
        }

        public async Task<byte[]> GetPdfForAutomatedSearchAsync(List<SkillLookupModel> skillLookups, 
            string userId, CancellationToken cancellationToken)
        {
            var skills = await _coursesRepository.GetSkillsAsync(skillLookups.Select(s => s.SkillId), cancellationToken);
            var skillDtos = _mapper.Map(skills, skillLookups);
            var courseDtos = await this.GetCoursesByAutomatedSearchAsync(skillLookups, userId, cancellationToken);
            
            return this.GenerateSearchResultsPdf(skillDtos, courseDtos);
        }

        public async Task<byte[]> GetPdfForAutomatedSearchBasedOnTimeAsync(List<SkillLookupModel> skillLookups, 
            string userId, CancellationToken cancellationToken)
        {
            var skills = await _coursesRepository.GetSkillsAsync(skillLookups.Select(s => s.SkillId), cancellationToken);
            var skillDtos = _mapper.Map(skills, skillLookups);
            var courseDtos = await this.GetCoursesByAutomatedSearchBasedOnTimeAsync(skillLookups, userId, cancellationToken);
            
            return this.GenerateSearchResultsPdf(skillDtos, courseDtos);
        }

        private byte[] GenerateSearchResultsPdf(List<SkillDto> skillDtos, List<CourseShortDto> courseDtos)
        {
            using var reader = new PdfReader(_searchResultsPdfUrl);
            using var newFile = new MemoryStream();
            using var writer = new PdfWriter(newFile);
            using var pdf = new PdfDocument(reader, writer);

            var page = pdf.GetPage(1);
            var canvas = new PdfCanvas(page);

            var rows = 0;
            foreach (var skill in skillDtos)
            {
                canvas.BeginText()
                    .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 12)
                    .SetFillColor(new DeviceRgb(42, 44, 62))
                    .MoveText(17, 520 - rows * 20)
                    .ShowText($"{skill.Name} - {skill.Level}")
                    .EndText();

                rows++;
            }

            rows = 0;
            foreach (var course in courseDtos)
            {
                canvas.BeginText()
                    .SetFontAndSize(PdfFontFactory.CreateFont(_robotoSlabBold), 16)
                    .SetFillColor(new DeviceRgb(42, 44, 62))
                    .MoveText(217, 520 - rows * 65)
                    .ShowText(course.Name)
                    .EndText();

                canvas.BeginText()
                    .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 12)
                    .SetFillColor(new DeviceRgb(42, 44, 62))
                    .MoveText(217, 505 - rows * 65)
                    .ShowText($"Updated on {course.UpdateDateUTC:MMM dd, yyyy}")
                    .EndText();

                canvas.BeginText()
                    .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA), 10)
                    .SetFillColor(new DeviceRgb(226, 120, 247))
                    .MoveText(217, 490 - rows * 65)
                    .ShowText($"{AppHttpContext.BaseUrl}/courses/{course.Id}")
                    .EndText();

                rows++;
            }

            pdf.Close();

            return newFile.ToArray();
        }

        private List<CourseLookupModel> ProcessCourseLookups(List<CourseLookupModel> courseLookups, 
            List<SkillLookupModel> skillLookups)
        {
            var processed = new List<CourseLookupModel>();
            foreach (var lookup in courseLookups)
            {
                var courseSkills = new List<CoursesSkills>();
                foreach (var cs in lookup.CoursesSkills)
                {
                    var skillLookup = skillLookups.FirstOrDefault(s => s.SkillId == cs.SkillId);
                    courseSkills.Add(new CoursesSkills
                    {
                        Level = Math.Min(cs.Level, skillLookup.Level),
                    });
                }
                processed.Add(new CourseLookupModel
                {
                    CourseId = lookup.CourseId,
                    Levels = courseSkills.Sum(cs => cs.Level),
                    CoursesSkills = lookup.CoursesSkills,
                    MaterialIds = lookup.MaterialIds,
                    LearningTime = lookup.LearningTime,
                });
            }

            return processed;
        }
    }
}
