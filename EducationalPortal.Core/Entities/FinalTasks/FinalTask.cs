using EducationalPortal.Core.Common;

namespace EducationalPortal.Core.Entities.FinalTasks
{
    public class FinalTask : EntityBase
    {
        public string Name { get; set; }

        public string Text { get; set; }

        public DateTime ReviewDeadlineTime { get; set; }

        public List<ReviewQuestion> ReviewQuestions { get; set; }

        public List<SubmittedFinalTask> SubmittedFinalTasks { get; set; }

        public List<Course> Courses { get; set; }
    }
}
