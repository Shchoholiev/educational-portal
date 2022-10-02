using EducationalPortal.Application.Models.DTO.Course;

namespace EducationalPortal.Application.Models.DTO
{
    public class UserDto
    {
        public string Name { get; set; }

        public string Position { get; set; }

        public string Email { get; set; }

        public string Avatar { get; set; }

        public int Balance { get; set; }

        public List<UsersSkillsDto> UsersSkills { get; set; }

        public List<CourseShortDto> CreatedCourses { get; set; }
    }
}
