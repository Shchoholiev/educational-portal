namespace EducationalPortal.Application.Models.QueryModels
{
    public class CourseShortQueryModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Thumbnail { get; set; }

        public string ShortDescription { get; set; }

        public int Price { get; set; }

        public DateTime UpdateDateUTC { get; set; }

        public int StudentsCount { get; set; }
    }
}
