using FIFA_API.Contracts.Repository;
using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace FIFA_API.Utils
{
    public static class ControllerExtensions
    {
        /// <summary>
        /// Tries to get the current user (<see cref="Models.EntityFramework.Utilisateur"/>) of the request.
        /// </summary>
        /// <param name="controller">The controller to get the user from.</param>
        /// <returns>The user of the request.</returns>
        public static async Task<Utilisateur?> UtilisateurAsync(this ControllerBase controller)
        {
            var idClaim = controller.User.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim is null || !int.TryParse(idClaim.Value, out int userId)) return null;

            IUtilisateurRepository repo = controller.HttpContext.RequestServices.GetService<IUtilisateurRepository>()!;
            return await repo.GetByIdAsync(userId);
        }
    }
}
