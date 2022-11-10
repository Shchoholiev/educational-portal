namespace EducationalPortal.Application.Models.DTO.Course
{
    public class CourseShortDto : BaseDto
    {
        public string Name { get; set; }

        public string Thumbnail { get; set; }

        public string ShortDescription { get; set; }

        public int Price { get; set; }

        public DateTime UpdateDateUTC { get; set; }

        public int StudentsCount { get; set; }

        public bool IsBought { get; set; }
    }
}
