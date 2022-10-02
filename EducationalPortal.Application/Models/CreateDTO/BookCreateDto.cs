using EducationalPortal.Application.Models.DTO;
using EducationalPortal.Application.Models.DTO.EducationalMaterials.Properties;
using Microsoft.AspNetCore.Http;

namespace EducationalPortal.Application.Models.CreateDTO
{
    public class BookCreateDto : MaterialBaseDto
    {
        public IFormFile File { get; set; }

        public int PagesCount { get; set; }

        public int PublicationYear { get; set; }

        public List<AuthorDto> Authors { get; set; }
    }
}
