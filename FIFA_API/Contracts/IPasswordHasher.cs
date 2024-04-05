namespace FIFA_API.Contracts
{
    /// <summary>
    /// Interface utilisée pour hasher les mots de passe.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Hashe le mot de passe.
        /// </summary>
        /// <param name="password">Le mot de passe à hasher.</param>
        /// <returns>Un hash du mot de passe.</returns>
        string Hash(string password);

        /// <summary>
        /// Vérifie si un mot de passe correspond à un hash.
        /// </summary>
        /// <param name="hash">Le hash à vérifier.</param>
        /// <param name="password">Le mot de passe à tester.</param>
        /// <returns><see langword="true"/> si le mot de passe correspond au hash, <see langword="false"/> sinon.</returns>
        bool Verify(string hash, string password);
    }
}
