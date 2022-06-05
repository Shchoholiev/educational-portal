using EducationalPortal.Core.Common;
using EducationalPortal.Core.Entities.JoinEntities;

namespace EducationalPortal.Core.Entities
{
    public class Skill : EntityBase
    {
        public string Name { get; set; }

        public List<Course> Courses { get; set; }

        public List<UsersSkills> UsersSkills { get; set; }
    }
}
