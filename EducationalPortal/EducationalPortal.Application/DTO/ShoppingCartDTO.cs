using EducationalPortal.Core.Entities;

namespace EducationalPortal.Application.DTO
{
    public class ShoppingCartDTO
    {
        public List<Course> Courses { get; set; }

        public int TotalPrice { get; set; }
    }
}
