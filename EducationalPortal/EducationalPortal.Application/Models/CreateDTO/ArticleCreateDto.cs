using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;

namespace EducationalPortal.Application.Models.DTO.EducationalMaterials
{
    public class ArticleCreateDto : MaterialBaseDto
    {
        public string Link { get; set; }

        public ResourceDto Resource { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
