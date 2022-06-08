using System.ComponentModel.DataAnnotations;

namespace EducationalPortal.Core.Common
{
    public abstract class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}