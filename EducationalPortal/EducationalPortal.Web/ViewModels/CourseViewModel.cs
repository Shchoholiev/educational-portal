using EducationalPortal.Core.Entities;

namespace EducationalPortal.Web.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Thumbnail { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public List<Skill> Skills { get; set; }

        public List<MaterialsBaseViewModel> Materials { get; set; }
    }
}
