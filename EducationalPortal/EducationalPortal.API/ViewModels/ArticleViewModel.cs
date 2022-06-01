using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.API.ViewModels
{
    public class ArticleViewModel : MaterialsBaseViewModel
    {
        public Resource Resource { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
