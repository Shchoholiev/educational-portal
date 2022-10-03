using EducationalPortal.Core.Common;

namespace EducationalPortal.Core.Entities
{
    public class Role : EntityBase
    {
        public string Name { get; set; }

        public List<User> Users { get; set; }
    }
}
