using FIFA_API.Models.EntityFramework;
using System.Security.Claims;

namespace FIFA_API.Contracts
{
    /// <summary>
    /// Interface utilisée pour authentifier des utilisateurs avec des jetons JWT.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Génère un jeton d'accès au compte.
        /// </summary>
        /// <param name="user">L'utilisateur pour qui générer le jeton d'accès.</param>
        /// <returns>Un jeton d'accès JWT au compte.</returns>
        string GenerateAccessToken(Utilisateur user);

        /// <summary>
        /// Génère un jeton d'actualisation, pour regénérer un jeton d'accès.
        /// </summary>
        /// <returns>Un jeton d'actualisation.</returns>
        string GenerateRefreshToken();

        /// <summary>
        /// Retourne l'utilisateur à partir de son <see cref="ClaimsPrincipal"/>.
        /// </summary>
        /// <param name="principal">Les claims de l'utilisateur.</param>
        /// <returns>L'utilisateur trouvé, <see langword="null"/> sinon.</returns>
        Task<Utilisateur?> GetUserFromPrincipalAsync(ClaimsPrincipal principal);

        /// <summary>
        /// Retourne l'utilisateur associé à un jeton d'accès (à jour ou expiré).
        /// </summary>
        /// <param name="token">Un jeton d'accès JWT, à jour ou expiré.</param>
        /// <returns>L'utilisateur trouvé, <see langword="null"/> sinon.</returns>
        Task<Utilisateur?> GetUserFromExpiredAsync(string token);
    }
}
