namespace EducationalPortal.Application.Models.DTO.FinalTasks
{
    public class FinalTaskDto : BaseDto
    {
        public string Name { get; set; }

        public string Text { get; set; }

        public DateTime ReviewDeadlineTime { get; set; }

        public List<ReviewQuestionDto> ReviewQuestions { get; set; }
    }
}
