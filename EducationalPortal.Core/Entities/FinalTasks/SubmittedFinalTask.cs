using EducationalPortal.Core.Common;

namespace EducationalPortal.Core.Entities.FinalTasks
{
    public class SubmittedFinalTask : EntityBase
    {
        public string FileLink { get; set; }

        public DateTime SubmitDateUTC { get; set; }

        public int Mark { get; set; }

        public User? RevievedBy { get; set; }

        public int FinalTaskId { get; set; }

        public FinalTask FinalTask { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public List<SubmittedReviewQuestion>? SubmittedReviewQuestions { get; set; }
    }
}
