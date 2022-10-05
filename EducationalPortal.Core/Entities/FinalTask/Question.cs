using EducationalPortal.Core.Common;

namespace EducationalPortal.Core.Entities.FinalTask
{
    public class Question : EntityBase
    {
        public string Text { get; set; }

        public int MaxMark { get; set; }

        public FinalTask FinalTask { get; set; }
    }
}
