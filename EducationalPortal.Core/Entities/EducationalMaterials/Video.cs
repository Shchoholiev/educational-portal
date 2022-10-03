using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.Core.Entities.EducationalMaterials
{
    public class Video : MaterialsBase
    {
        public DateTime Duration { get; set; }

        public Quality Quality { get; set; }
    }
}
