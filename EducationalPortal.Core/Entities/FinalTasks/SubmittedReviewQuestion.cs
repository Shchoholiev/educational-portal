namespace EducationalPortal.Core.Entities.FinalTasks
{
    public class SubmittedReviewQuestion
    {
        public int Mark { get; set; }

        public int QuestionId { get; set; }

        public ReviewQuestion Question { get; set; }

        public int SubmittedFinalTaskId { get; set; }

        public SubmittedFinalTask SubmittedFinalTask { get; set; }
    }
}
