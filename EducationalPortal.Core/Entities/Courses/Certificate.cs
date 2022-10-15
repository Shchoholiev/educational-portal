using EducationalPortal.Core.Common;

namespace EducationalPortal.Core.Entities.Courses
{
    public class Certificate : EntityBase
    {
        public Guid VerificationCode { get; set; }

        public DateTime DateOfCompletionUTC { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }
    }
}
