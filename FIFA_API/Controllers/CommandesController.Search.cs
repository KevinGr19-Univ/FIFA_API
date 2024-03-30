using FIFA_API.Models;
using FIFA_API.Models.Controllers;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Controllers
{
    public partial class CommandesController
    {
        public const int COMMANDES_PER_PAGE = 20;

        [HttpGet("search")]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<ApercuCommande>>> SearchCommandes(
            [FromQuery] int? idUser,
            [FromQuery] int[] typesLivraison,
            [FromQuery] bool? desc,
            [FromQuery] int? page)
        {
            var query = _context.Commandes as IQueryable<Commande>;

            if (idUser is not null)
                query = query.Where(c => c.IdUtilisateur == idUser);

            if (typesLivraison.Length > 0)
                query = query.Where(c => typesLivraison.Contains(c.IdTypeLivraison));

            return Ok(await query
                .Sort(desc == true)
                .Paginate(Math.Max(page ?? 1, 1), COMMANDES_PER_PAGE)
                .ToApercus());
        }

        [HttpGet("me")]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<IEnumerable<ApercuCommande>>> SearchMyCommands(
            [FromQuery] int[] typesLivraison,
            [FromQuery] bool? desc,
            [FromQuery] int? page)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            return await SearchCommandes(user.Id, typesLivraison, desc, page);
        }
    }
}
