using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using Microsoft.AspNetCore.Http;

namespace EducationalPortal.Application.Models.DTO.EducationalMaterials
{
    public class VideoCreateDto : MaterialBaseDto
    {
        public IFormFile File { get; set; }

        public QualityDto Quality { get; set; }

        public int Duration { get; set; }
    }
}
