using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;

namespace EducationalPortal.Application.Models.DTO.EducationalMaterials
{
    public class VideoDto : MaterialBaseDto
    {
        public QualityDto Quality { get; set; }

        public DateTime Duration { get; set; }
    }
}
