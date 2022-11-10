namespace EducationalPortal.Application.Models.QueryModels.Statistics
{
    public class CourseStatisticsQueryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime UpdateDateUTC { get; set; }

        public int Price { get; set; }

        public int StudentsCount { get; set; }
    }
}
