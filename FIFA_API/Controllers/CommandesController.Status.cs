using FIFA_API.Models;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Controllers
{
    public partial class CommandesController
    {
        /// <summary>
        /// Ajoute un status à une commande.
        /// </summary>
        /// <remarks>NOTE: Certains status nécessitent différents roles.</remarks>
        /// <param name="status">Le status à ajouter.</param>
        /// <returns>Le status ajouté.</returns>
        /// <response code="401">Accès refusé.</response>
        [HttpPost("status")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<StatusCommande>> PostStatus([FromBody] StatusCommande status)
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

            bool authRes = await this.MatchPolicyAsync(role);
            if (!authRes) return Unauthorized();

            int lastStatusCode = (int?)lastStatus?.Code ?? -1;
            if ((int)status.Code - lastStatusCode != 1) return Conflict();

            status.Date = DateTime.Now;
            commande.Status.Add(status);
            await _context.SaveChangesAsync();

            return status;
        }

    }
}
