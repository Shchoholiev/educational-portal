using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.JoinEntities;

namespace EducationalPortal.Core.Entities
{
    public class User : EntityBase
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        
        public Role Role { get; set; }

        public List<MaterialsBase> Materials { get; set; }

        public List<UsersSkills> UsersSkills { get; set; }
    }
}
