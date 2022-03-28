using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.Application.DTO
{
    public class ArticleDTO
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public Resource Resource { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
