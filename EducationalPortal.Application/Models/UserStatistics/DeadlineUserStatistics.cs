using EducationalPortal.Core.Enums;

namespace EducationalPortal.Application.Models.UserStatistics
{
    public class DeadlineUserStatistics
    {
        public DateTime DateTimeUTC { get; set; }

        public DeadlineTypes DeadlineType { get; set; }
    }
}
