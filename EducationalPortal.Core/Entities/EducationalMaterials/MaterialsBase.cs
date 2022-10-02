using EducationalPortal.Core.Common;
using EducationalPortal.Core.Entities.JoinEntities;

namespace EducationalPortal.Core.Entities.EducationalMaterials
{
    public class MaterialsBase : EntityBase
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public List<CoursesMaterials?> CoursesMaterials { get; set; }
         
        public List<User?> Users { get; set; }
    }
}
