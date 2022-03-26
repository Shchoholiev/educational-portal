using EducationalPortal.Core.Entities.EducationalMaterials.Properties;

namespace EducationalPortal.Application.DTO
{
    public class BookDTO : MaterialBaseDTO
    {
        public int PagesCount { get; set; }

        public int PublicationYear { get; set; }

        public List<Author> Authors { get; set; }
    }
}
