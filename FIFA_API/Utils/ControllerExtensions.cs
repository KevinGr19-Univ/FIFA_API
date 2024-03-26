using FIFA_API.Contracts;
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
            ITokenService tokenService = controller.HttpContext.RequestServices.GetService<ITokenService>()!;
            return await tokenService.GetUserFromPrincipalAsync(controller.User);
        }
    }
}
