using FIFA_API.Contracts;
using Isopoh.Cryptography.Argon2;
using System.Text;

namespace FIFA_API.Services
{
    public class Argon2PasswordHasher : IPasswordHasher
    {
        private const Argon2Type ARGON2_TYPE = Argon2Type.HybridAddressing;

        private readonly byte[]? _secret;

        public Argon2PasswordHasher(byte[]? secret = null)
        {
            _secret = secret;
        }

        public string Hash(string password)
        {
            return Argon2.Hash(
                password: Encoding.UTF8.GetBytes(password),
                type: ARGON2_TYPE,
                secret: _secret
            );
        }

        public bool Verify(string hash, string password)
        {
            return Argon2.Verify(
                encoded: hash,
                configToVerify: new Argon2Config()
                {
                    Password = Encoding.UTF8.GetBytes(password),
                    Type = ARGON2_TYPE,
                    Secret = _secret
                }
            );
        }
    }
}
