using EducationalPortal.Core.Common;

namespace EducationalPortal.Core.Entities
{
    public class UserToken : EntityBase
    {
        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
