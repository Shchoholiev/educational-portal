namespace EducationalPortal.Core.Entities.JoinEntities
{
    public class UsersCourses
    {
        public string UserId { get; set; }

        public User User { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public int MaterialsCount { get; set; }

        public int LearnedMaterialsCount { get; set; }
    }
}
