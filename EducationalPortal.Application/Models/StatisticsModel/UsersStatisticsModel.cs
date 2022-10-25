using EducationalPortal.Application.Paging;

namespace EducationalPortal.Application.Models.StatisticsModel
{
    public class UsersStatisticsModel
    {
        public PagedList<UserStatisticsModel> Users { get; set; }

        public int AverageCompletedCoursesPercentage { get; set; }
    }
}
