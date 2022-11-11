namespace EducationalPortal.Application.Models.UserStatistics
{
    public class LearningUserStatistics
    {
        public DateOnly Date { get; set; }

        public int LearnedMaterialsCount { get; set; }

        public bool HasDeadline { get; set; }
    }
}
