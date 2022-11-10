namespace EducationalPortal.Application.Models.DTO
{
    public class MaterialBaseDto : BaseDto
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public bool IsLearned { get; set; }

        public int LearningMinutes { get; set; }
    }
}
