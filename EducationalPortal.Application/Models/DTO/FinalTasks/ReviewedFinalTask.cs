namespace EducationalPortal.Application.Models.DTO.FinalTasks
{
    public class ReviewedFinalTask
    {
        public int SubmittedFinalTaskId { get; set; }

        public string ReviewerId { get; set; }

        public List<SubmittedReviewQuestionDto> SubmittedReviewQuestions { get; set; }
    }
}
