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

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class JoueursController : ControllerBase
    {
        /// <summary>
        /// Le nom de la <see cref="AuthorizationPolicy"/> requise pour les actions de modifications.
        /// </summary>
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly FifaDbContext _context;

        public JoueursController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/Joueurs
        /// <summary>
        /// Retourne la liste des joueurs.
        /// </summary>
        /// <returns>La liste des joueurs.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Joueur>>> GetJoueurs()
        {
            return await _context.Joueurs.ToListAsync();
        }

        // GET: api/Joueurs/5
        /// <summary>
        /// Retourne un joueur.
        /// </summary>
        /// <param name="id">L'id du joueur recherché.</param>
        /// <returns>Le joueur recherché.</returns>
        /// <response code="404">Le joueur recherché n'existe pas.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Joueur>> GetJoueur(int id)
        {
            var joueur = await _context.Joueurs.GetByIdAsync(id);

            if (joueur == null)
            {
                return NotFound();
            }

            return joueur;
        }

        // PUT: api/Joueurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie un joueur.
        /// </summary>
        /// <param name="id">L'id du joueur à modifier.</param>
        /// <param name="joueur">Les nouvelles informations du joueur.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le joueur recherché n'existe pas.</response>
        /// <response code="400">Les nouvelles informations du joueur sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutJoueur(int id, Joueur joueur)
        {
            if (id != joueur.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(joueur);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JoueurExists(id))
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

        // POST: api/Joueurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute un joueur.
        /// </summary>
        /// <param name="joueur">Le joueur à ajouter.</param>
        /// <returns>Le nouveau joueur.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">Le nouveau joueur est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Joueur>> PostJoueur(Joueur joueur)
        {
            await _context.Joueurs.AddAsync(joueur);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJoueur", new { id = joueur.Id }, joueur);
        }

        // DELETE: api/Joueurs/5
        /// <summary>
        /// Supprime un joueur.
        /// </summary>
        /// <param name="id">L'id du joueur à supprimer.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le joueur recherché n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteJoueur(int id)
        {
            var joueur = await _context.Joueurs.FindAsync(id);
            if (joueur == null)
            {
                return NotFound();
            }

            _context.Joueurs.Remove(joueur);
            if(joueur.Stats is not null)
                _context.Statistiques.Remove(joueur.Stats);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool JoueurExists(int id)
        {
            return (_context.Joueurs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
