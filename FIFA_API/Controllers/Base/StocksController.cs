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
using FIFA_API.Models;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class StocksController : ControllerBase
    {
        /// <summary>
        /// Le nom de la <see cref="AuthorizationPolicy"/> requise pour modifier des stocks de produits.
        /// </summary>
        public const string EDIT_POLICY = Policies.Admin;

        /// <summary>
        /// Le nom de la <see cref="AuthorizationPolicy"/> requise pour supprimer des stocks de produits.
        /// </summary>
        public const string DELETE_POLICY = Policies.Admin;

        private readonly IManagerStockProduit _manager;

        public StocksController(IManagerStockProduit manager)
        {
            _manager = manager;
        }

        // GET: api/Stocks
        /// <summary>
        /// Retourne la liste des stocks.
        /// </summary>
        /// <remarks>NOTE: Requiert les droits d'édition de stocks.</remarks>
        /// <returns>La liste des stocks.</returns>
        /// <response code="401">Accès refusé.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = EDIT_POLICY)]
        public async Task<ActionResult<IEnumerable<StockProduit>>> GetStockProduits()
        {
            return Ok(await _manager.GetAll());
        }

        // GET: api/Stocks/5
        /// <summary>
        /// Retourne un stock de produit.
        /// </summary>
        /// <param name="idvariante">L'id de la variante de produit.</param>
        /// <param name="idtaille">L'id de la taille de produit.</param>
        /// <returns>Le stock de produit de cette variante et taille.</returns>
        /// <response code="404">Le stock recherché n'existe pas.</response>
        [HttpGet("{idvariante}/{idtaille}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StockProduit>> GetStockProduit(int idvariante, int idtaille)
        {
            var stockProduit = await _manager.GetById(idvariante, idtaille);

            if (stockProduit == null)
            {
                return NotFound();
            }

            return Ok(stockProduit);
        }

        // PUT: api/Stocks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie un stock de produit.
        /// </summary>
        /// <remarks>NOTE: Requiert les droits d'édition de stocks.</remarks>
        /// <param name="idvariante">L'id de la variante de produit.</param>
        /// <param name="idtaille">L'id de la taille de produit.</param>
        /// <param name="stockProduit">Les nouvelles informations du stock de produit.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le stock recherché n'existe pas.</response>
        /// <response code="400">Les nouvelles informations du stock sont invalides.</response>
        [HttpPut("{idvariante}/{idtaille}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = EDIT_POLICY)]
        public async Task<IActionResult> PutStockProduit(int idvariante, int idtaille, [FromBody] StockProduit stockProduit)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (idvariante != stockProduit.IdVCProduit || idtaille != stockProduit.IdTaille)
            {
                return BadRequest();
            }

            if (!await _manager.Exists(idvariante, idtaille))
            {
                return NotFound();
            }

            await _manager.Update(stockProduit);
            await _manager.Save();

            return NoContent();
        }

        // POST: api/Stocks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute un stock de produit.
        /// </summary>
        /// <remarks>NOTE: Requiert les droits d'édition de stocks.</remarks>
        /// <param name="stockProduit">Le stock de produit à ajouter.</param>
        /// <returns>Le nouveau stock de produit.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">Le stock de produit est invalide.</response>
        /// <response code="409">Un stock de produit existe déjà pour la variante et la taille données.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = EDIT_POLICY)]
        public async Task<ActionResult<StockProduit>> PostStockProduit(StockProduit stockProduit)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (await _manager.Exists(stockProduit.IdVCProduit, stockProduit.IdTaille))
            {
                return Conflict();
            }

            await _manager.Add(stockProduit);
            await _manager.Save();

            return CreatedAtAction("GetStockProduit", new { idvariante = stockProduit.IdVCProduit, idtaille = stockProduit.IdTaille }, stockProduit);
        }

        // DELETE: api/Stocks/5
        /// <summary>
        /// Supprime un stock de produit.
        /// </summary>
        /// <remarks>NOTE: Requiert les droits de suppression de stocks.</remarks>
        /// <param name="idvariante">L'id de la variante de produit.</param>
        /// <param name="idtaille">L'id de la taille de produit.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le stock recherché n'existe pas.</response>
        [HttpDelete("{idvariante}/{idtaille}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = DELETE_POLICY)]
        public async Task<IActionResult> DeleteStockProduit(int idvariante, int idtaille)
        {
            var stockProduit = await _manager.GetById(idvariante, idtaille);
            if (stockProduit == null)
            {
                return NotFound();
            }

            await _manager.Delete(stockProduit);
            await _manager.Save();

            return NoContent();
        }
    }
}
