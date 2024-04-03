using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using FIFA_API.Utils;

namespace FIFA_API.Controllers
{
    [Route("api/tailles")]
    [ApiController]
    public partial class TaillesController : ControllerBase
    {
        private readonly FifaDbContext _context;

        public TaillesController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/TailleProduits
        /// <summary>
        /// Retourne la liste des tailles.
        /// </summary>
        /// <remarks>NOTE: La requête filtre les instances en fonction du niveau de permission.</remarks>
        /// <returns>La liste des tailles.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TailleProduit>>> GetTailleProduits()
        {
            IQueryable<TailleProduit> query = _context.TailleProduits;
            if (!await this.MatchPolicyAsync(ProduitsController.SEE_POLICY)) query = query.FilterVisibles();
            return await query.ToListAsync();
        }

        // GET: api/TailleProduits/5
        /// <summary>
        /// Retourne une taille.
        /// </summary>
        /// <remarks>NOTE: La requête filtre les instances en fonction du niveau de permission.</remarks>
        /// <param name="id">L'id de la taille.</param>
        /// <returns>La taille recherchée.</returns>
        /// <response code="404">La taille recherchée n'existe pas ou a été filtrée.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TailleProduit>> GetTailleProduit(int id)
        {
            var tailleProduit = await _context.TailleProduits.FindAsync(id);

            if (tailleProduit == null) return NotFound();
            if (!tailleProduit.Visible
                && !await this.MatchPolicyAsync(ProduitsController.SEE_POLICY))
                return NotFound();

            return tailleProduit;
        }

        // PUT: api/TailleProduits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie une taille.
        /// </summary>
        /// <param name="id">L'id de la taille.</param>
        /// <param name="tailleProduit">Les nouvelles informations de la taille.</param>
        /// <remarks>NOTE: Requiert les droits d'édition de produit.</remarks>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La taille recherchée n'existe pas.</response>
        /// <response code="400">Les nouvelles informations de la taille sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = ProduitsController.EDIT_POLICY)]
        public async Task<IActionResult> PutTailleProduit(int id, TailleProduit tailleProduit)
        {
            if (id != tailleProduit.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(tailleProduit);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TailleProduitExists(id))
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

        // POST: api/TailleProduits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute une nouvelle taille.
        /// </summary>
        /// <param name="tailleProduit">La taille à ajouter.</param>
        /// <remarks>NOTE: Requiert les droits d'ajout de produit.</remarks>
        /// <returns>La nouvelle taille.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">La nouvelle taille est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = ProduitsController.ADD_POLICY)]
        public async Task<ActionResult<TailleProduit>> PostTailleProduit(TailleProduit tailleProduit)
        {
            await _context.TailleProduits.AddAsync(tailleProduit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTailleProduit", new { id = tailleProduit.Id }, tailleProduit);
        }

        // DELETE: api/TailleProduits/5
        /// <summary>
        /// Supprime une taille.
        /// </summary>
        /// <param name="id">L'id de la taille à supprimer.</param>
        /// <remarks>NOTE: Requiert les droits de suppression de produit.</remarks>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La taille recherchée n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = ProduitsController.DELETE_POLICY)]
        public async Task<IActionResult> DeleteTailleProduit(int id)
        {
            var tailleProduit = await _context.TailleProduits.FindAsync(id);
            if (tailleProduit == null)
            {
                return NotFound();
            }

            _context.TailleProduits.Remove(tailleProduit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TailleProduitExists(int id)
        {
            return (_context.TailleProduits?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
