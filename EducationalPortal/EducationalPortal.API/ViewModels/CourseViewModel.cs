using EducationalPortal.Application.Models.DTO;

namespace EducationalPortal.API.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Thumbnail { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public UserDto Author { get; set; }

        public List<SkillDto> Skills { get; set; }

        public List<MaterialsBaseViewModel> Materials { get; set; }
    }
}
