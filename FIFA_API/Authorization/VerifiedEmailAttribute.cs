using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FIFA_API.Authorization
{
    /// <summary>
    /// Filtre autorisant seulement les utilisateurs ayant une adresse email vérifiée.
    /// </summary>
    public class VerifiedEmailAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Utilisateur? user = await context.HttpContext.UtilisateurAsync();
            if (user is null)
            {
                context.Result = new UnauthorizedResult();
            }

            else if (!user.VerifEmail)
            {
                context.Result = new UnauthorizedObjectResult(new { verified_email_required = true });
            }

            await Task.CompletedTask;
        }
    }
}
