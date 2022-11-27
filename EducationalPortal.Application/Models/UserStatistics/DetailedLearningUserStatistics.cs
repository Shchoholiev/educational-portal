namespace EducationalPortal.Application.Models.UserStatistics
{
    public class DetailedLearningUserStatistics
    {
        public List<string> MaterialsNames { get; set; }

        public List<DeadlineUserStatistics> Deadlines { get; set; }
    }
}
