namespace EducationalPortal.Application.Models.QueryModels.Statistics
{
    public class UserStatisticsQueryModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public int Balance { get; set; }

        public int BoughtCoursesCount { get; set; }

        public int CompletedCoursesCount { get; set; }
    }
}
