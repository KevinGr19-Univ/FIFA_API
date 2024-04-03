using FIFA_API.Models;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Controllers
{
    public partial class CommandesController
    {
        [HttpPost("status")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<StatusCommande>> PostStatus([FromBody] StatusCommande status, [FromServices] IAuthorizationService authorizationService)
        {
            var commande = await _context.Commandes.GetByIdAsync(status.IdCommande);
            if (commande is null) return NotFound();

            var lastStatus = commande.Status.MaxBy(s => s.Date);

            string role = lastStatus?.Code switch
            {
                CodeStatusCommande.Preparation => Policies.ServiceCommande,
                CodeStatusCommande.Validation => Policies.ServiceExpedition,
                _ => Policies.Admin
            };

            var authRes = await authorizationService.AuthorizeAsync(User, role);
            if (!authRes.Succeeded) return Unauthorized();

            int lastStatusCode = (int?)lastStatus?.Code ?? -1;
            if ((int)status.Code - lastStatusCode != 1) return Conflict();

            status.Date = DateTime.Now;
            commande.Status.Add(status);
            await _context.SaveChangesAsync();

            return status;
        }

    }
}
