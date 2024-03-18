using FIFA_API.Contracts;
using Isopoh.Cryptography.Argon2;
using System.Text;

namespace FIFA_API.Services
{
    public class Argon2PasswordHasher : IPasswordHasher
    {
        private const Argon2Type ARGON_TYPE = Argon2Type.DataIndependentAddressing;

        public string Hash(string password)
        {
            return Argon2.Hash(password, type: ARGON_TYPE);
        }

        public bool Verify(string hash, string password)
        {
            var config = new Argon2Config()
            {
                Password = Encoding.UTF8.GetBytes(password),
                Type = ARGON_TYPE
            };
            return Argon2.Verify(hash, config);
        }
    }
}
