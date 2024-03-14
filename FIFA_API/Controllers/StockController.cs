using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Models.Repository;
using NuGet.Protocol.Core.Types;

namespace FIFA_API.Controllers
{

    /// <summary>
    /// Controleur pour gérer les opérations sur les joueurs.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IRepository<StockProduit> _repository;

        public StockController(IRepository<StockProduit> repository)
        {
            _repository = repository;   
        }

        // GET: api/Stock
        /// <summary>
        /// Recupere la liste des stocks.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockProduit>>> GetStockList()
        {
            return await _repository.GetAllAsync();
        }

        // GET: api/Stock/1/1
        /// <summary>
        /// Recupere le stock avec idvariante et idtaille.
        /// </summary>
        /// <param name="idvariante">L'id variante à recuperer.</param>
        /// <param name="idtaille">L'id taille à recuperer.</param>
        /// <returns>Le stock correspondant.</returns>
        [HttpGet("{idvariante}/{idtaille}")]
        [ActionName("GetStock")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<StockProduit>> GetStock(int idvariante, int idtaille)
        {
            var stock = await _repository.GetByIdAsync(idvariante, idtaille);

            if (stock == null)
            {
                return NotFound();
            }

            return stock.Value;
        }

        // PUT: api/Stock/1/1
        /// <summary>
        /// Met à jour un stock existant.
        /// </summary>
        /// <param name="idvariante">L'id variante du stock à mettre à jour.</param>
        /// <param name="idtaille">L'id taille du stock à mettre à jour.</param>
        /// <param name="stock">Les nouvelles données mis à jour du stock.</param>
        [HttpPut("{idvariante}/{idtaille}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutJoueur(int idvariante, int idtaille, StockProduit stock)
        {

            if (idvariante != stock.IdVCProduit && idtaille != stock.IdTaille)
            {
                return BadRequest();
            }

            try
            {
                var oldStock = await _repository.GetByIdAsync(idvariante, idtaille);
                await _repository.UpdateAsync(oldStock.Value, stock);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Stock
        /// <summary>
        /// Ajoute un nouveau stock.
        /// </summary>
        /// <param name="stock">Les données du stock à ajouter.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<StockProduit>> PostStock(StockProduit stock)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.AddAsync(stock);

            return CreatedAtAction("GetJoueur", new { idvariante = stock.IdVCProduit, idtaille = stock.IdTaille }, stock);
        }

        // DELETE: api/Stock/1/1
        /// <summary>
        /// Supprime un stock existant.
        /// </summary>
        /// <param name="id">L'id du stock à supprimer.</param>
        [HttpDelete("{idvariante}/{idtaille}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteJoueur(int idvariante, int idtaille)
        {
            var result = await _repository.GetByIdAsync(idvariante, idtaille);

            if (result == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(result.Value);

            return NoContent();
        }
    }
}
