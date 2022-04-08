using EducationalPortal.Application.DTO.EducationalMaterials.Properties;
using Microsoft.AspNetCore.Http;

namespace EducationalPortal.Application.DTO.EducationalMaterials
{
    public class BookDTO : MaterialBaseDTO
    {
        public IFormFile File { get; set; }

        public int PagesCount { get; set; }

        public int PublicationYear { get; set; }

        public List<AuthorDTO> Authors { get; set; }
    }
}
