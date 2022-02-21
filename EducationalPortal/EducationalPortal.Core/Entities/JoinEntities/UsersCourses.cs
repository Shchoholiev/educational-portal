namespace EducationalPortal.Core.Entities.JoinEntities
{
    public class UsersCourses
    {
        public User User { get; set; }

        public Course Course { get; set; }

        public int Progress { get; set; }
    }
}
