namespace EducationalPortal.Core.Entities.FinalTask
{
    public class QuestionSubmittedFinalTask
    {
        public int Mark { get; set; }

        public int QuestionId { get; set; }

        public Question Question { get; set; }

        public int SubmittedFinalTaskId { get; set; }

        public SubmittedFinalTask SubmittedFinalTask { get; set; }
    }
}
