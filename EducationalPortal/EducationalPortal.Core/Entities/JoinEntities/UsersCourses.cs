namespace EducationalPortal.Core.Entities.JoinEntities
{
    public class UsersCourses
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public int Progress { get; set; }
    }
}
