using System.Security.Claims;

namespace FIFA_API.Contracts
{
    // Classe Wrapper/Proxy créée pour être mockable, contrairement à IAuthorizationService

    /// <summary>
    /// Interface utilisée pour regrouper et englober (Wrapper) les méthodes d'authorisation customisées.
    /// </summary>
    public interface ICustomAuthorizationService
    {
        Task<bool> MatchPolicyAsync(ClaimsPrincipal user, string policy);
    }
}
