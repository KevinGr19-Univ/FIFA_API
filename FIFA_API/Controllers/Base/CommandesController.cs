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
        private readonly FifaDbContext _context;

        public CommandesController(FifaDbContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
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
            return await _context.Commandes.ToListAsync();
        }

        // GET: api/Commandes/5
        /// <summary>
        /// Retourne une commande.
        /// </summary>
        /// <param name="id">L'id de la commande.</param>
        /// <param name="authService">Le service d'authorisation à utiliser.</param>
        /// <returns>La commande recherchée.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La commande n'existe pas ou n'appartient pas à l'utilisateur.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<CommandeDetails>> GetCommande(int id, [FromServices] IAuthorizationService authService)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            var commande = await _context.Commandes.GetByIdAsync(id);

            if (commande is null) return NotFound();
            if (commande.IdUtilisateur != user.Id)
            {
                var authRes = await authService.AuthorizeAsync(User, MANAGER_POLICY);
                if(!authRes.Succeeded) return NotFound();
            }

            return await CommandeDetails.FromCommande(commande, _context);
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
            if (id != commande.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(commande);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommandeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

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
            await _context.Commandes.AddAsync(commande);
            await _context.SaveChangesAsync();

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
            var commande = await _context.Commandes.FindAsync(id);
            if (commande == null)
            {
                return NotFound();
            }

            _context.Commandes.Remove(commande);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommandeExists(int id)
        {
            return (_context.Commandes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
