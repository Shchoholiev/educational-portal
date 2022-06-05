using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;

namespace EducationalPortal.Application.Models.CreateDTO
{
    public class ArticleCreateDto : MaterialBaseDto
    {
        public string Link { get; set; }

        public ResourceDto Resource { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
