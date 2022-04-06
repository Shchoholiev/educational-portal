using EducationalPortal.Application.DTO.EducationalMaterials.Properties;

namespace EducationalPortal.Application.DTO.EducationalMaterials
{
    public class BookDTO : MaterialBaseDTO
    {
        public int PagesCount { get; set; }

        public int PublicationYear { get; set; }

        public List<AuthorDTO> Authors { get; set; }
    }
}
