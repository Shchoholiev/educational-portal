namespace EducationalPortal.Application.Interfaces.Identity
{
    public interface IPasswordHasher
    {
        string Hash(string password);

        bool Check(string password, string passwordHash);
    }
}
