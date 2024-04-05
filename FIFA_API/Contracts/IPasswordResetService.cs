using FIFA_API.Models.Controllers;

namespace FIFA_API.Contracts
{
    /// <summary>
    /// Interface utilisée pour réinitialiser les mots de passe des utilisateurs.
    /// </summary>
    public interface IPasswordResetService
    {
        /// <summary>
        /// Envoie un mail de vérification.
        /// </summary>
        /// <param name="mail">L'adresse mail à qui envoyer la requête.</param>
        Task SendPasswordResetCodeAsync(string mail);

        /// <summary>
        /// Réinitialise le mot de passe de l'utilisateur associé à la requête.
        /// </summary>
        /// <param name="request">L'adresse mail du compte et le nouveau mot de passe.</param>
        /// <param name="code">Le code de réinitialisation envoyé.</param>
        /// <returns><see langword="true"/> si le mot de passe a été réinitialisé, <see langword="false"/> sinon.</returns>
        Task<bool> ChangePasswordAsync(ChangePasswordRequest request, string code);
    }
}
