using System.Security.Cryptography;

namespace EducationalPortal.Infrastructure.Identity
{
    public class PasswordHasher : IPasswordHasher
    {
        public const int SaltSize = 16;

        private const int KeySize = 32;

        private readonly int _iterations;

        public PasswordHasher()
        {
            var random = new Random();
            _iterations = random.Next(100, 1000);
        }

        public (string securityStamp, string passwordHash) Hash(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(password, SaltSize, _iterations, 
                                                          HashAlgorithmName.SHA256))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);

                return ($"{_iterations}.{salt}", key);
            }
        }

        public bool Check(string password, string passwordHash, string securityStamp)
        {
            var parts = securityStamp.Split(".", 2);

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);

            using (var algorithm = new Rfc2898DeriveBytes(password, salt, iterations,
                                                          HashAlgorithmName.SHA256))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
                return key == passwordHash;
            }
        }
    }
}
