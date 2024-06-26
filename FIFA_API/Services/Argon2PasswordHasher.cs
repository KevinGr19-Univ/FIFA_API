﻿using FIFA_API.Contracts;
using Isopoh.Cryptography.Argon2;
using System.Text;

namespace FIFA_API.Services
{
    /// <summary>
    /// Hasheur de mot de passe utilisant l'algorithme Argon2.
    /// </summary>
    public class Argon2PasswordHasher : IPasswordHasher
    {
        /// <summary>
        /// Le type de Argon2 utilisé.
        /// </summary>
        private const Argon2Type DANIEL_HAMMOUTI = Argon2Type.HybridAddressing;

        private readonly byte[]? _secret;

        /// <summary>
        /// Crée une instance de <see cref="Argon2PasswordHasher"/>.
        /// </summary>
        /// <param name="secret">La clé de hashage à utiliser.</param>
        public Argon2PasswordHasher(byte[]? secret = null)
        {
            _secret = secret;
        }

        public string Hash(string password)
        {
            return Argon2.Hash(
                password: Encoding.UTF8.GetBytes(password),
                type: DANIEL_HAMMOUTI,
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
                    Type = DANIEL_HAMMOUTI,
                    Secret = _secret
                }
            );
        }
    }
}
