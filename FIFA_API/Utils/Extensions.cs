using FIFA_API.Contracts;
using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Utils
{
    public static class Extensions
    {
        /// <inheritdoc cref="UtilisateurAsync(HttpContext)"/>
        public static async Task<Utilisateur?> UtilisateurAsync(this ControllerBase controller)
        {
            return await controller.HttpContext.UtilisateurAsync();
        }

        /// <summary>
        /// Tente de récupérer l'utilisateur de la requête courrante.
        /// </summary>
        /// <returns>L'utilisateur de la requête, <see langword="null"/> sinon.</returns>
        public static async Task<Utilisateur?> UtilisateurAsync(this HttpContext context)
        {
            ITokenService tokenService = context.RequestServices.GetService<ITokenService>()!;
            var user = await tokenService.GetUserFromPrincipalAsync(context.User);

            if (user is null) return null;
            return user.Anonyme ? null : user;
        }

        public static async Task<bool> MatchPolicyAsync(this ControllerBase controller, string policy)
        {
            ICustomAuthorizationService authService = controller.HttpContext.RequestServices.GetService<ICustomAuthorizationService>()!;
            return await authService.MatchPolicyAsync(controller.User, policy);
        }
    }
}
