using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.API.ViewModels.CreateViewModels
{
    public class BookCreateModel : MaterialsBaseCreateModel
    {
        public int PagesCount { get; set; }

        public Extension Extension { get; set; }

        public int PublicationYear { get; set; }

        public List<Author> Authors { get; set; }
    }
}
