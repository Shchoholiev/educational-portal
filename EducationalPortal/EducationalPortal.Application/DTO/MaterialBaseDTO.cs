using Microsoft.AspNetCore.Http;

namespace EducationalPortal.Application.DTO
{
    public class MaterialBaseDTO
    {
        public string Name { get; set; }

        public IFormFile File { get; set; }
    }
}
