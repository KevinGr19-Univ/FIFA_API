using FIFA_API.Models.EntityFramework;

namespace FIFA_API.Contracts
{
    /// <summary>
    /// Interface utilisée dans le processus de vérification des adresses mail des utilisateurs.
    /// </summary>
    public interface IEmailVerificator
    {
        /// <summary>
        /// Envoie un mail de vérification à l'utilisateur.
        /// </summary>
        /// <param name="user">L'utilisateur à vérifier.</param>
        Task SendVerificationAsync(Utilisateur user);

        /// <summary>
        /// Vérifie l'adresse mail d'un utilisateur avec le code envoyé.
        /// </summary>
        /// <param name="user">L'utilisateur à vérifier.</param>
        /// <param name="code">Le code de vérification envoyé.</param>
        /// <returns><see langword="true"/> si la vérification s'est bien effectuée, <see langword="false"/> sinon.</returns>
        Task<bool> VerifyAsync(Utilisateur user, string code);
    }
}
