using EducationalPortal.Application.DTO.EducationalMaterials.Properties;

namespace EducationalPortal.Application.DTO
{
    public class VideoDTO : MaterialBaseDTO
    {
        public QualityDTO Quality { get; set; }

        public int Duration { get; set; }
    }
}
