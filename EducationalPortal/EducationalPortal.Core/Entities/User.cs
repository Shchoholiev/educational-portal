using EducationalPortal.Core.Entities.EducationalMaterials;
using EducationalPortal.Core.Entities.JoinEntities;
using System.ComponentModel.DataAnnotations;

namespace EducationalPortal.Core.Entities
{
    public class User : EntityBase
    {
        [Key]
        public new string Id { get; set; }

        public string Name { get; set; }

        public string? Position { get; set; }

        public string Avatar { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public int Balance { get; set; }

        public UserToken? UserToken { get; set; }

        public List<Role> Roles { get; set; }

        public List<MaterialsBase> Materials { get; set; }

        public List<UsersSkills> UsersSkills { get; set; }

        public List<UsersCourses> UsersCourses { get; set; }

        public List<Course?> CreatedCourses { get; set; }
    }
}
