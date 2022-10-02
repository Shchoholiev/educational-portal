using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;

namespace EducationalPortal.Application.Models.DTO.EducationalMaterials
{
    public class BookDto : MaterialBaseDto
    {
        public int PagesCount { get; set; }

        public int PublicationYear { get; set; }

        public ExtensionDto Extension { get; set; }

        public List<AuthorDto> Authors { get; set; }
    }
}
