namespace FIFA_API.Contracts
{
    /// <summary>
    /// Contract used to hash and verify passwords.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Hashes the password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>A hash of the password.</returns>
        string Hash(string password);

        /// <summary>
        /// Verifies if a given password matches an hash.
        /// </summary>
        /// <param name="hash">The hash of the original password.</param>
        /// <param name="password">The password to test.</param>
        /// <returns><see langword="true"/> if the password matches the hash, <see langword="false"/> otherwise.</returns>
        bool Verify(string hash, string password);
    }
}
