using FIFA_API.Models.EntityFramework;
using System.Security.Claims;

namespace FIFA_API.Contracts
{
    /// <summary>
    /// Contract used to generate access and refresh tokens.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generates an access token to authenticate as a <see cref="Utilisateur"/>.
        /// </summary>
        /// <param name="user">The user to generate a token for.</param>
        /// <returns>A string access token.</returns>
        string GenerateAccessToken(Utilisateur user);

        /// <summary>
        /// Generates a refresh token, used to get another access token without providing credentials.
        /// </summary>
        /// <returns>A string refresh token.</returns>
        string GenerateRefreshToken();

        /// <summary>
        /// Gets the user <see cref="Utilisateur"/> from a <see cref="ClaimsPrincipal"/>.
        /// </summary>
        /// <param name="principal">The principal of the request.</param>
        /// <returns>The principal's user if found, <see langword="null"/> otherwise.</returns>
        Task<Utilisateur?> GetUserFromPrincipalAsync(ClaimsPrincipal principal);

        /// <summary>
        /// Gets the user (<see cref="Utilisateur"/>) from an expired access token.
        /// </summary>
        /// <param name="token">The string expired access token.</param>
        /// <returns>The token's user if found, <see langword="null"/> otherwise.</returns>
        Task<Utilisateur?> GetUserFromExpiredAsync(string token);
    }
}
