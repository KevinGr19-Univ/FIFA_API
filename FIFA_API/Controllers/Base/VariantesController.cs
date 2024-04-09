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
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class VariantesController : ControllerBase
    {
        private readonly IManagerVarianteCouleurProduit _manager;

        public VariantesController(IManagerVarianteCouleurProduit manager)
        {
            _manager = manager;
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
            bool seeAll = await this.MatchPolicyAsync(ProduitsController.SEE_POLICY);
            return Ok(await _manager.GetAll(!seeAll));
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
            bool seeAll = await this.MatchPolicyAsync(ProduitsController.SEE_POLICY);
            var variante = await _manager.GetByIdWithData(id, !seeAll);

            if (variante is null) return NotFound();
            return Ok(variante);
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
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != variante.Id) return BadRequest();

            var oldVariante = await _manager.GetById(id, false);
            if (oldVariante is null) return NotFound();

            if (oldVariante.IdCouleur != variante.IdCouleur || oldVariante.IdProduit != variante.IdProduit)
                return BadRequest();

            await _manager.Update(variante);
            await _manager.Save();
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
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (await _manager.CombinationExists(variante.IdProduit, variante.IdCouleur))
                return Conflict();

            await _manager.Add(variante);
            await _manager.Save();

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
        /// <response code="403">La variante recherchée est utilisée dans des stocks.</response>
        /// <response code="404">La variante recherchée n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = ProduitsController.DELETE_POLICY)]
        public async Task<IActionResult> DeleteVariante(int id)
        {
            var variante = await _manager.GetByIdWithStocks(id, false);
            if (variante is null) return NotFound();

            if (variante.Stocks.Count > 0) return Forbid();

            await _manager.Delete(variante);
            await _manager.Save();

            return NoContent();
        }
    }
}
