namespace EducationalPortal.Application.Models.DTO.Course
{
    public class CourseLearnDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Progress { get; set; }

        public List<MaterialBaseDto> Materials { get; set; }
    }
}
