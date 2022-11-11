using EducationalPortal.Core.Entities.EducationalMaterials;

namespace EducationalPortal.Core.Entities.JoinEntities
{
    public class UserMaterial
    {
        public string UserId { get; set; }

        public User User { get; set; }

        public int MaterialId { get; set; }

        public MaterialsBase Material { get; set; }

        public DateTime LearnDateUTC { get; set; }
    }
}
