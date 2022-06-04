using EducationalPortal.Core.Common;

namespace EducationalPortal.Core.Entities
{
    public class CartItem : EntityBase
    {
        public User User { get; set; }

        public Course Course { get; set; }
    }
}
