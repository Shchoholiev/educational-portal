namespace EducationalPortal.Application.Models.DTO.FinalTasks
{
    public class SubmittedFinalTaskDto
    {
        public string FileLink { get; set; }

        public DateTime SubmitDateUTC { get; set; }

        public int FinalTaskId { get; set; }

        public string UserId { get; set; }

        public int Mark { get; set; }

        public UserDto? ReviewedBy { get; set; }

        public bool ReviewedTask { get; set; }

        public List<SubmittedReviewQuestionDto>? SubmittedReviewQuestions { get; set; }
    }
}
