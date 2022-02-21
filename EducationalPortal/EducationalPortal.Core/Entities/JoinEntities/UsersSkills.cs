namespace EducationalPortal.Core.Entities.JoinEntities
{
    public class UsersSkills
    {
        public User User { get; set; }

        public Skill Skill { get; set; }

        public int Level { get; set; }
    }
}
