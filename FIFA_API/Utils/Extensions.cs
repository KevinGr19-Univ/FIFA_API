using FIFA_API.Contracts;
using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Utils
{
    public static class Extensions
    {
        /// <summary>
        /// Tries to get the current user (<see cref="Models.EntityFramework.Utilisateur"/>) of the request.
        /// </summary>
        /// <param name="controller">The controller to get the user from.</param>
        /// <returns>The user of the request.</returns>
        public static async Task<Utilisateur?> UtilisateurAsync(this ControllerBase controller)
        {
            return await controller.HttpContext.UtilisateurAsync();
        }

        /// <summary>
        /// Tries to get the current user (<see cref="Models.EntityFramework.Utilisateur"/>) of the request.
        /// </summary>
        /// <param name="context">The context of the request.</param>
        /// <returns>The user of the request.</returns>
        public static async Task<Utilisateur?> UtilisateurAsync(this HttpContext context)
        {
            ITokenService tokenService = context.RequestServices.GetService<ITokenService>()!;
            var user = await tokenService.GetUserFromPrincipalAsync(context.User);

            if (user is null) return null;
            return user.Anonyme ? null : user;
        }
    }
}
