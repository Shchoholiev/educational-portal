using EducationalPortal.Core.Common;

namespace EducationalPortal.Core.Entities
{
    public class ShoppingHistory : EntityBase
    {
        public User User { get; set; }

        public Course Course { get; set; }

        public int Price { get; set; }

        public DateTime Date { get; set; }
    }
}
