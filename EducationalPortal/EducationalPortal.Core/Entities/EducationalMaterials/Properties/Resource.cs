using EducationalPortal.Core.Common;

namespace EducationalPortal.Core.Entities.EducationalMaterials.Properties
{
    public class Resource : EntityBase
    {
        public string Name { get; set; }

        public List<Article> Articles { get; set; }
    }
}
