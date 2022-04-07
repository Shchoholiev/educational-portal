using EducationalPortal.Application.DTO.EducationalMaterials.Properties;
using Microsoft.AspNetCore.Http;

namespace EducationalPortal.Application.DTO
{
    public class VideoDTO : MaterialBaseDTO
    {
        public IFormFile File { get; set; }

        public QualityDTO Quality { get; set; }

        public int Duration { get; set; }
    }
}
