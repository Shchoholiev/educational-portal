using EducationalPortal.Application.DTO.EducationalMaterials.Properties;

namespace EducationalPortal.Application.DTO.EducationalMaterials
{
    public class ArticleDTO
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public ResourceDTO Resource { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
