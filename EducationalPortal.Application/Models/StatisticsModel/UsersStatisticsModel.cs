namespace EducationalPortal.Application.Models.StatisticsModel
{
    public class UsersStatisticsModel
    {
        public List<UserStatisticsModel> Users { get; set; }

        public int AverageCompletedCoursesPercentage { get; set; }
    }
}
