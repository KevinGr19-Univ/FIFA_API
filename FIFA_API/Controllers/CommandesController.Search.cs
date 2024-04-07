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

        /// <summary>
        /// Retourne une liste de commandes à partir de critères de recherche.
        /// </summary>
        /// <param name="idUser">L'id de l'utilisateur.</param>
        /// <param name="typesLivraison">Les ids des types de livraison.</param>
        /// <param name="desc">Si le tri doit être décroissant.</param>
        /// <param name="page">Le numéro de page à utiliser pour paginer le résultat.</param>
        /// <returns>Une liste de commandes correspondantes.</returns>
        /// <response code="401">Accès refusé.</response>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<ApercuCommande>>> SearchCommandes(
            [FromQuery] int? idUser,
            [FromQuery] int[] typesLivraison,
            [FromQuery] bool? desc,
            [FromQuery] int? page)
        {
            return Ok(await _uow.Commandes.SearchCommandes(
                idUser,
                typesLivraison,
                desc,
                page ?? 1,
                COMMANDES_PER_PAGE));
        }

        /// <summary>
        /// Retourne une liste de commandes de l'utilisateur à partir de critères de recherche.
        /// </summary>
        /// <param name="typesLivraison">Les ids des types de livraison.</param>
        /// <param name="desc">Si le tri doit être décroissant.</param>
        /// <param name="page">Le numéro de page à utiliser pour paginer le résultat.</param>
        /// <returns>Une liste de commandes correspondantes de l'utilisateur.</returns>
        /// <response code="401">Accès refusé.</response>
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
