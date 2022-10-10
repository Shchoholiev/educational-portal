namespace EducationalPortal.Application.Models.DTO.FinalTasks
{
    public class FinalTaskForReview
    {
        public int SubmittedFinalTaskId { get; set; }

        public string FileLink { get; set; }

        public string FinalTaskText { get; set; }

        public IEnumerable<ReviewQuestionDto> ReviewQuestions { get; set; }
    }
}
