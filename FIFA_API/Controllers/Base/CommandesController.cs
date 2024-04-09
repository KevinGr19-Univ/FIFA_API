using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using FIFA_API.Models;
using Microsoft.AspNetCore.Authorization;
using FIFA_API.Models.Controllers;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class CommandesController : ControllerBase
    {
        /// <summary>
        /// Le nom de la <see cref="AuthorizationPolicy"/> requise pour les actions de modifications.
        /// </summary>
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly IConfiguration _config;
        private readonly IUnitOfWorkCommande _uow;

        public CommandesController(IUnitOfWorkCommande uow, IConfiguration config)
        {
            _config = config;
            _uow = uow;
        }

        // GET: api/Commandes
        /// <summary>
        /// Retourne la liste des commandes.
        /// </summary>
        /// <returns>La liste des commandes.</returns>
        /// <response code="401">Accès refusé</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<Commande>>> GetCommandes()
        {
            return Ok(await _uow.Commandes.GetAll());
        }

        // GET: api/Commandes/5
        /// <summary>
        /// Retourne une commande.
        /// </summary>
        /// <param name="id">L'id de la commande.</param>
        /// <returns>La commande recherchée.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La commande n'existe pas ou n'appartient pas à l'utilisateur.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<CommandeDetails>> GetCommande(int id)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            var commande = await _uow.Commandes.GetByIdWithAll(id);

            if (commande is null) return NotFound();
            if (commande.IdUtilisateur != user.Id)
            {
                var authRes = await this.MatchPolicyAsync(MANAGER_POLICY);
                if(!authRes) return NotFound();
            }

            return Ok(await _uow.GetDetails(commande));
        }

        // PUT: api/Commandes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie une commande.
        /// </summary>
        /// <param name="id">L'id de la commande.</param>
        /// <param name="commande">Les nouvelles informations de la commande.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La commande n'existe pas.</response>
        /// <response code="400">Les nouvelles informations de la commande sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutCommande(int id, Commande commande)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != commande.Id)
            {
                return BadRequest();
            }

            if (!await _uow.Commandes.Exists(id))
            {
                return NotFound();
            }

            await _uow.Commandes.Update(commande);
            await _uow.SaveChanges();

            return NoContent();
        }

        // POST: api/Commandes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute une commande.
        /// </summary>
        /// <remarks>NOTE: Cette méthode ne devrait pas être utilisée pour un usage commun.</remarks>
        /// <param name="commande">La commande à ajouter.</param>
        /// <returns>La nouvelle commande.</returns>
        /// <response code="401">Accès refusé</response>
        /// <response code="400">Les nouvelles informations de la commande sont invalides.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Commande>> PostCommande(Commande commande)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _uow.Commandes.Add(commande);
            await _uow.SaveChanges();

            return CreatedAtAction("GetCommande", new { id = commande.Id }, commande);
        }

        // DELETE: api/Commandes/5
        /// <summary>
        /// Supprime une commande.
        /// </summary>
        /// <param name="id">L'id de la commande.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé</response>
        /// <response code="404">La commande recherchée n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteCommande(int id)
        {
            var commande = await _uow.Commandes.GetById(id);
            if (commande == null)
            {
                return NotFound();
            }

            await _uow.Commandes.Delete(commande);
            await _uow.SaveChanges();

            return NoContent();
        }
    }
}
