using EducationalPortal.Application.Models.DTO.Course;

namespace EducationalPortal.Application.Models.DTO
{
    public class CartItemDto : BaseDto
    {
        public CourseShortDto Course { get; set; }

        public string UserEmail { get; set; }
    }
}
