namespace EducationalPortal.Infrastructure.Identity
{
    public interface IPasswordHasher
    {
        (string securityStamp, string passwordHash) Hash(string password);

        bool Check(string password, string passwordHash, string securityStamp);
    }
}
