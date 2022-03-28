using EducationalPortal.Core.Entities;
using EducationalPortal.Core.Entities.EducationalMaterials;
using Microsoft.AspNetCore.Http;

namespace EducationalPortal.Application.DTO
{
    public class CourseDTO
    {
        public string Name { get; set; }

        public IFormFile Thumbnail { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public List<Skill> Skills { get; set; }

        public List<MaterialsBase> Materials { get; set; }
    }
}
