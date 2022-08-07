using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.Web.ViewModels
{
    public class BookViewModel : MaterialsBaseViewModel
    {
        public int PagesCount { get; set; }

        public string Extension { get; set; }

        public int PublicationYear { get; set; }

        public List<Author> Authors { get; set; }
    }
}
