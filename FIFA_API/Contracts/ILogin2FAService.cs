using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts
{
    /// <summary>
    /// Interface utilisée pour authentifier des utilisateurs par 2FA.
    /// </summary>
    public interface ILogin2FAService
    {
        /// <summary>
        /// Supprime le code existant pour cet utilisateur.
        /// </summary>
        /// <param name="user">L'utilisateur du code à supprimer.</param>
        /// <returns><see langword="true"/> si un code a été supprimé, <see langword="false"/> sinon.</returns>
        Task<bool> Remove2FACode(Utilisateur user);

        /// <summary>
        /// Envoie un code de 2FA à l'utilisateur.
        /// </summary>
        /// <param name="user">L'utilisateur à authentifier.</param>
        /// <returns>Le jeton de 2FA à utiliser avec le code.</returns>
        Task<string> Send2FACodeAsync(Utilisateur user);

        /// <summary>
        /// Retourne l'utilisateur associé au jeton et au code de 2FA.
        /// </summary>
        /// <param name="token">Le jeton de 2FA.</param>
        /// <param name="code">Le code de 2FA.</param>
        /// <returns>L'utilisateur associé aux informations passées.</returns>
        Task<Utilisateur?> AuthenticateAsync(string token, string code);
    }
}
