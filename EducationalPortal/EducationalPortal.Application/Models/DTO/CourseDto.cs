namespace EducationalPortal.Application.Models.DTO
{
    public class CourseDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Thumbnail { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public List<SkillDto> Skills { get; set; }

        public List<MaterialBaseDto> Materials { get; set; }
    }
}
