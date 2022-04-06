namespace EducationalPortal.Application.DTO
{
    public class CourseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Thumbnail { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public List<SkillDTO> Skills { get; set; }

        public List<MaterialBaseDTO> Materials { get; set; }
    }
}
