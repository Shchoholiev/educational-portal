using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;

namespace EducationalPortal.Application.Models.DTO.EducationalMaterials
{
    public class ArticleDto : MaterialBaseDto
    {
        public ResourceDto Resource { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
