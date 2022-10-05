namespace EducationalPortal.Core.Entities.FinalTask
{
    public class SubmittedFinalTask
    {
        public string FileLink { get; set; }

        public DateTime SubmitDateUTC { get; set; }

        public int Mark { get; set; }

        public User RevievedBy { get; set; }

        public int FInalTaskId { get; set; }

        public FinalTask FinalTask { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
