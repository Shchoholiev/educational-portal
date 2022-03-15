namespace EducationalPortal.Core.Entities
{
    public class ShoppingHistory : EntityBase
    {
        public User User { get; set; }

        public Course Course { get; set; }

        public double Price { get; set; }

        public DateTime Date { get; set; }
    }
}
