using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;

namespace FIFA_API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class VariantesController : ControllerBase
    {
        private readonly FifaDbContext _context;

        public VariantesController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/Variantes
        /// <summary>
        /// Retourne la liste des variantes.
        /// </summary>
        /// <remarks>NOTE: La requête filtre les instances en fonction du niveau de permission.</remarks>
        /// <returns>La liste des variantes.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VarianteCouleurProduit>>> GetVariantes()
        {
            IQueryable<VarianteCouleurProduit> query = _context.VarianteCouleurProduits;
            if (!await this.MatchPolicyAsync(ProduitsController.SEE_POLICY)) query = query.FilterVisibles();
            return await query.ToListAsync();
        }

        // GET: api/Variantes/5
        /// <summary>
        /// Retourne une variante.
        /// </summary>
        /// <remarks>NOTE: La requête filtre les instances en fonction du niveau de permission.</remarks>
        /// <param name="id">L'id de la variante recherchée.</param>
        /// <returns>La variante recherchée.</returns>
        /// <response code="404">La variante recherchée n'existe pas ou a été filtrée.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<VarianteCouleurProduit>> GetVariante(int id)
        {
            var variante = await _context.VarianteCouleurProduits.GetByIdAsync(id);
            if (variante is null) return NotFound();
            if (!variante.Visible
                && !await this.MatchPolicyAsync(ProduitsController.SEE_POLICY))
                return NotFound();

            return variante;
        }

        // PUT: api/Variantes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie une variante.
        /// </summary>
        /// <param name="id">L'id de la variante à modifier.</param>
        /// <param name="variante">Les nouvelles informations de la variante.</param>
        /// <remarks>NOTE: Requiert les droits d'édition de produit.</remarks>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé</response>
        /// <response code="404">La variante recherchée n'existe pas.</response>
        /// <response code="400">Les nouvelles informations de la variante sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = ProduitsController.EDIT_POLICY)]
        public async Task<IActionResult> PutVariante(int id, VarianteCouleurProduit variante)
        {
            if (id != variante.Id) return BadRequest();

            var oldVariante = await _context.VarianteCouleurProduits.FindAsync(id);
            if (oldVariante is null) return NotFound();

            if (oldVariante.IdCouleur != variante.IdCouleur || oldVariante.IdProduit != variante.IdProduit)
                return BadRequest();

            await _context.UpdateEntity(variante);
            return NoContent();
        }

        // POST: api/Variantes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute une nouvelle variante.
        /// </summary>
        /// <param name="variante">La variante à ajouter.</param>
        /// <remarks>NOTE: Requiert les droits d'édition de produit.</remarks>
        /// <returns>La nouvelle variante.</returns>
        /// <response code="401">Accès refusé</response>
        /// <response code="400">La nouvelle variante est invalide.</response>
        /// <response code="409">Une variante existe déjà pour la couleur et le produit donnés.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = ProduitsController.EDIT_POLICY)] // Adding color = Editing product
        public async Task<ActionResult<VarianteCouleurProduit>> PostVariante(VarianteCouleurProduit variante)
        {
            try
            {
                await _context.VarianteCouleurProduits.AddAsync(variante);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                bool combinationExists = await _context.VarianteCouleurProduits.AnyAsync(v => v.IdCouleur == variante.IdCouleur && v.IdProduit == variante.IdProduit);
                if (combinationExists) return Conflict();
                throw;
            }

            return CreatedAtAction("GetVariante", new { id = variante.Id }, variante);
        }

        // DELETE: api/Variantes/5
        /// <summary>
        /// Supprime une variante.
        /// </summary>
        /// <param name="id">L'id de la variante à supprimer.</param>
        /// <remarks>NOTE: Requiert les droits de suppression de produit.</remarks>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé</response>
        /// <response code="404">La variante recherchée n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = ProduitsController.DELETE_POLICY)]
        public async Task<IActionResult> DeleteVariante(int id)
        {
            var variante = await _context.VarianteCouleurProduits.FindAsync(id);
            if (variante is null) return NotFound();

            bool stocksExists = await _context.StockProduits.AnyAsync(s => s.IdVCProduit == id);
            if (stocksExists) return Forbid();

            _context.VarianteCouleurProduits.Remove(variante);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
