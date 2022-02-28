namespace EducationalPortal.Core.Entities.JoinEntities
{
    public class UsersSkills
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int SkillId { get; set; }

        public Skill Skill { get; set; }

        public int Level { get; set; }
    }
}
