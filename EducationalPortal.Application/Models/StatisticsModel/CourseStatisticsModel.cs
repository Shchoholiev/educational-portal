namespace EducationalPortal.Application.Models.StatisticsModel
{
    public class CourseStatisticsModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime UpdateDateUTC { get; set; }

        public int Price { get; set; }

        public int StudentsCount { get; set; }
    }
}
