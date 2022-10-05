using EducationalPortal.Core.Common;

namespace EducationalPortal.Core.Entities.FinalTask
{
    public class FinalTask : EntityBase
    {
        public string Text { get; set; }

        public DateTime DeadlineTime { get; set; }

        public List<Question> Questions { get; set; }
    }
}
