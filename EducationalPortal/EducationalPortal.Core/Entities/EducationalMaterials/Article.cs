using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.Core.Entities.EducationalMaterials
{
    public class Article : MaterialsBase
    {
        public Resource Resource { get; set; }

        public DateOnly PublicationDate { get; set; }
    }
}
