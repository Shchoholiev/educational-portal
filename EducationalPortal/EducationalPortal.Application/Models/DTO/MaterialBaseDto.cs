namespace EducationalPortal.Application.Models.DTO
{
    public class MaterialBaseDto : EntityBaseDto
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public bool IsLearned { get; set; }
    }
}
