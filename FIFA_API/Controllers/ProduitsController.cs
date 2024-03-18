using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Models.Repository;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduitsController : ControllerBase
    {
        private readonly IRepository<Produit> _dataRepository;

        public ProduitsController(BaseRepository<Produit> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        // GET: api/Produits
        /// <summary>
        /// Récupérer tous les produits.
        /// </summary>
        /// <returns>Http response</returns>
        /// <response code="200">Les produits ont été récupéré.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produit>>> GetProduits()
        {
            var produits = await _dataRepository.GetAllAsync();
            return Ok(produits);
        }

        // GET: api/Produits/5
        /// <summary>
        /// Récupérer un produit grâce à son id.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="id">L'id du produit que vous voulez récupérer.</param>
        /// <response code="200">Le produit a été récupéré.</response>
        /// <response code="404">Le produit n'a pas été trouvé.</response>
        [HttpGet("{id}")]
        [ActionName("GetProduitById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Produit>> GetProduitById(int id)
        {
            var result = await _dataRepository.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // PUT: api/Produits/5
        /// <summary>
        /// Modifier un produit.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="id">L'id du produit que vous voulez modifier.</param>
        /// <param name="produit">La nouveau produit.</param>
        /// <response code="204">Le produit a été modifié.</response>
        /// <response code="404">Le produit n'a pas été trouvé.</response>
        /// <response code="400">Le nouveau produit n'a pas le même id que l'ancien.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutProduit(int id, Produit produit)
        {
            if (id != produit.Id)
            {
                return BadRequest();
            }

            var produitToUpdate = await _dataRepository.GetByIdAsync(id);
            if (produitToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await _dataRepository.UpdateAsync(produitToUpdate, produit);
                return NoContent();
            }
        }

        // POST: api/Produits
        /// <summary>
        /// Insérer un produit.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="produit">Le produit à insérer.</param>
        /// <response code="201">Le produit a été inséré.</response>
        /// <response code="400">Le produit n'a pas les paramètres nécessaires pour être inséré.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Produit>> PostProduit(Produit produit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _dataRepository.AddAsync(produit);
            return CreatedAtAction(nameof(GetProduitById), new { id = produit.Id }, produit);
        }

        // DELETE: api/Produits/5
        /// <summary>
        /// Supprimer un produit grâce à son id.
        /// </summary>
        /// <returns>Http response</returns>
        /// <param name="id">L'id du produit que vous voulez supprimer.</param>
        /// <response code="204">Le produit a été supprimé.</response>
        /// <response code="404">Le produit n'a pas été trouvé.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            var produit = await _dataRepository.GetByIdAsync(id);
            if (produit == null)
            {
                return NotFound();
            }
            await _dataRepository.DeleteAsync(produit);
            return NoContent();
        }

        //private bool ProduitExists(int id)
        //{
        //    return (_context.Produits?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
