using FIFA_API.Models;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Controllers
{
    public partial class CommandesController
    {
        [HttpGet("UserCommands/{idUtilisateur}")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<Commande>>> GetUserCommands(int idUtilisateur, [FromQuery] bool? desc)
        {
            Utilisateur? user = await _context.Utilisateurs.GetByIdAsync(idUtilisateur);
            if (user is null) return NotFound();

            return Ok(SortedCommands(user.Commandes, desc));
        }

        [HttpGet("MyCommands")]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<IEnumerable<Commande>>> GetMyCommands([FromQuery] bool? desc)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            return Ok(SortedCommands(user.Commandes, desc));
        }

        [HttpGet("MyCommand/{idcommande}")]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<Commande>> GetMyCommand(int idCommande)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            Commande? commande = await _context.Commandes.GetByIdAsync(idCommande);
            if (commande is null || commande.IdUtilisateur != user.Id) return NotFound();

            return Ok(commande);
        }

        private static IEnumerable<Commande> SortedCommands(IEnumerable<Commande> commands, bool? desc)
            => desc == true ? commands.OrderBy(c => c.DateCommande) : commands.OrderByDescending(c => c.DateCommande);
    }
}
