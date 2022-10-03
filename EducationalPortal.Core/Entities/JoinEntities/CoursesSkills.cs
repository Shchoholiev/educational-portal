namespace EducationalPortal.Core.Entities.JoinEntities
{
    public class CoursesSkills
    {
        public int CourseId { get; set; }

        public Course Course { get; set; }

        public int SkillId { get; set; }

        public Skill Skill { get; set; }
    }
}
