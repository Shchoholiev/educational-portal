using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.Application.DTO
{
    public class VideoDTO : MaterialBaseDTO
    {
        public Quality Quality { get; set; }

        public int Duration { get; set; }
    }
}
