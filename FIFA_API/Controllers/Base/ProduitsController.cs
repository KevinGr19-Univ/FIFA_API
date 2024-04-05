using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Models;
using Microsoft.AspNetCore.Authorization;
using FIFA_API.Utils;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class ProduitsController : ControllerBase
    {
        /// <summary>
        /// Le nom de la <see cref="AuthorizationPolicy"/> requise pour voir tous les produits.
        /// </summary>
        public const string SEE_POLICY = Policies.DirecteurVente;

        /// <summary>
        /// Le nom de la <see cref="AuthorizationPolicy"/> requise pour ajouter des produits.
        /// </summary>
        public const string ADD_POLICY = Policies.DirecteurVente;

        /// <summary>
        /// Le nom de la <see cref="AuthorizationPolicy"/> requise pour modifier des produits.
        /// </summary>
        public const string EDIT_POLICY = Policies.DirecteurVente;

        /// <summary>
        /// Le nom de la <see cref="AuthorizationPolicy"/> requise pour supprimer des produits.
        /// </summary>
        public const string DELETE_POLICY = Policies.Admin;

        private readonly FifaDbContext _context;

        public ProduitsController(FifaDbContext context)
        {
            _context = context;
        }

        // GET: api/Produits
        /// <summary>
        /// Retourne la liste des produits.
        /// </summary>
        /// <returns>La liste des produits.</returns>
        /// <response code="401">Accès refusé.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = SEE_POLICY)]
        public async Task<ActionResult<IEnumerable<Produit>>> GetProduits()
        {
            return await _context.Produits .ToListAsync();
        }

        // GET: api/Produits/5
        /// <summary>
        /// Retourne un produit.
        /// </summary>
        /// <remarks>NOTE: La requête filtre les instances en fonction du niveau de permission.</remarks>
        /// <param name="id">L'id du produit recherché.</param>
        /// <returns>Le produit recherché.</returns>
        /// <response code="404">Le produit recherché n'existe pas ou a été filtré.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Produit>> GetProduit(int id)
        {
            var produit = await _context.Produits.GetByIdAsync(id);

            if (produit is null)  return NotFound();
            if (!produit.Visible
                && !await this.MatchPolicyAsync(SEE_POLICY))
                return NotFound();

            return produit;
        }

        // PUT: api/Produits/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie un produit.
        /// </summary>
        /// <param name="id">L'id du produit à modifier.</param>
        /// <param name="produit">Les nouvelles informations du produit.</param>
        /// <remarks>NOTE: Requiert les droits d'édition de produit.</remarks>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le produit recherché n'existe pas.</response>
        /// <response code="400">Les nouvelles informations du produit sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = EDIT_POLICY)]
        public async Task<IActionResult> PutProduit(int id, Produit produit)
        {
            if (id != produit.Id)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(produit);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProduitExists(id))
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

        // POST: api/Produits
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute un produit.
        /// </summary>
        /// <param name="produit">Le produit à ajouter.</param>
        /// <remarks>NOTE: Requiert les droits d'ajout de produit.</remarks>
        /// <returns>Le nouveau produit.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">Le nouveau produit est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = ADD_POLICY)]
        public async Task<ActionResult<Produit>> PostProduit(Produit produit)
        {
            await _context.Produits.AddAsync(produit);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduit", new { id = produit.Id }, produit);
        }

        // DELETE: api/Produits/5
        /// <summary>
        /// Supprime un produit.
        /// </summary>
        /// <param name="id">L'id du produit recherché.</param>
        /// <remarks>NOTE: Requiert les droits de suppression de produit.</remarks>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le produit recherché n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = DELETE_POLICY)]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }

            _context.Produits.Remove(produit);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProduitExists(int id)
        {
            return (_context.Produits?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
