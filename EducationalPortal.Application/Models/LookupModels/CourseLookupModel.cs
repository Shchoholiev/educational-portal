using EducationalPortal.Core.Entities.JoinEntities;

namespace EducationalPortal.Application.Models.LookupModels
{
    public class CourseLookupModel
    {
        public int CourseId { get; set; }

        public int Levels { get; set; }

        public List<CoursesSkills> CoursesSkills { get; set; }

        public List<int> MaterialIds { get; set; }

        public int LearningTime { get; set; }
    }
}
