using EducationalPortal.Core.Entities.EducationalMaterials;

namespace EducationalPortal.Core.Entities.JoinEntities
{
    public class CoursesMaterials
    {
        public Course Course { get; set; }

        public MaterialsBase Material { get; set; }

        public int Index { get; set; }
    }
}
