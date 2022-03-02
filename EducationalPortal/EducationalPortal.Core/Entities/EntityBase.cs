using System.ComponentModel.DataAnnotations;

namespace EducationalPortal.Core.Entities
{
    public class EntityBase
    {
        [Key]
        public int Id { get; set; }
    }
}