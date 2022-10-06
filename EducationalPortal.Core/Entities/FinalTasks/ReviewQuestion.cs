using EducationalPortal.Core.Common;

namespace EducationalPortal.Core.Entities.FinalTasks
{
    public class ReviewQuestion : EntityBase
    {
        public string Text { get; set; }

        public int MaxMark { get; set; }

        public FinalTask FinalTask { get; set; }
    }
}
