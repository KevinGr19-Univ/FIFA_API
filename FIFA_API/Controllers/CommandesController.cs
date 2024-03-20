using FIFA_API.Models;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Controllers
{
    public partial class CommandesController
    {
        [HttpGet("GetUserCommandes")]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<IEnumerable<Commande>>> GetUserCommandes()
        {
            var user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            return Ok(user.Commandes);
        }

        [HttpGet("GetUserCommande/{id}")]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<Commande>> GetUserCommande([FromRoute] int id)
        {
            var user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            Commande? commande = await _commandeManager.GetByIdAsync(id);
            if (commande is null || commande.IdUtilisateur != user.Id) return NotFound();

            return commande;
        }
    }
}
