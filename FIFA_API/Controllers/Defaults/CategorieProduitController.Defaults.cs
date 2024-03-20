using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using NuGet.Protocol.Core.Types;
using FIFA_API.Contracts.Repository;
using FIFA_API.Models;
using Microsoft.AspNetCore.Authorization;

namespace FIFA_API.Controllers
{

    /// <summary>
    /// Controleur pour gérer les opérations sur les joueurs.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public partial class CategorieProduitController : ControllerBase
    {
        private const string MANAGER_POLICY = Policies.Admin;

        private readonly ICategorieProduitManager _manager;

        public CategorieProduitController(ICategorieProduitManager manager)
        {
            _manager = manager;
        }

        // GET: api/Categories
        /// <summary>
        /// Recupere la liste des categories.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategorieProduit>>> GetCategories()
        {
            var categories = await _manager.GetAllAsync();
            return Ok(categories);
        }

        // GET: api/Categories/5
        /// <summary>
        /// Recupere un categorie par son ID.
        /// </summary>
        /// <param name="id">L'ID de la catégorie à recuperer.</param>
        /// <returns>La catégorie correspondant à l'ID.</returns>
        [HttpGet("{id}")]
        [ActionName("GetCategorieById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CategorieProduit>> GetCategorieById(int id)
        {
            var categorie = await _manager.GetByIdAsync(id);

            if (categorie is null)
            {
                return NotFound();
            }

            return categorie;
        }

        // PUT: api/Categories/5
        /// <summary>
        /// Met à jour un catégorie existant.
        /// </summary>
        /// <param name="id">L'ID de la catégorie à mettre à jour.</param>
        /// <param name="categorie">Les nouvelles données mis à jour de la catégorie.</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutCategorie(int id, CategorieProduit categorie)
        {

            if (id != categorie.Id)
            {
                return BadRequest();
            }

            var categorieToUpdate = await _manager.GetByIdAsync(id);
            if(categorieToUpdate is null)
            {
                return NotFound();
            }

            await _manager.UpdateAsync(categorieToUpdate, categorie);
            return NoContent();
        }

        // POST: api/Categories
        /// <summary>
        /// Ajoute un nouveau categorie.
        /// </summary>
        /// <param name="categorie">Les données de la catégorie à ajouter.</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<Joueur>> PostCategorie([FromBody] CategorieProduit categorie)
        {
            await _manager.AddAsync(categorie);
            return CreatedAtAction("GetCategorieById", new { id = categorie.Id }, categorie);
        }

        // DELETE: api/Categories/5
        /// <summary>
        /// Supprime la catégorie existant.
        /// </summary>
        /// <param name="id">L'ID de la catégorie à supprimer.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteCategorie(int id)
        {
            var categorie = await _manager.GetByIdAsync(id);
            if (categorie is null)
            {
                return NotFound();
            }

            await _manager.DeleteAsync(categorie);
            return NoContent();
        }

    }
}
