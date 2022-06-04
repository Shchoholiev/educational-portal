using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using Microsoft.AspNetCore.Http;

namespace EducationalPortal.Application.Models.DTO.EducationalMaterials
{
    public class BookDto : MaterialBaseDto
    {
        public IFormFile File { get; set; }

        public int PagesCount { get; set; }

        public int PublicationYear { get; set; }

        public List<AuthorDto> Authors { get; set; }
    }
}
