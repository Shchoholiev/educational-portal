using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.JoinEntities;

namespace EducationalPortal.Core.Entities
{
    public class Course : EntityBase
    {
        public string Name { get; set; }

        public string Thumbnail { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public List<Skill> Skills { get; set; }

        public List<MaterialsBase> Materials { get; set; }

        public List<User> Users { get; set; }

        public List<CoursesMaterials> CoursesMaterials { get; set; }

        public List<UsersCourses> UsersCourses { get; set; }
    }
}
