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
    public class CategorieController : ControllerBase
    {
        private readonly IRepository<CategorieProduit> _repository;

        public CategorieController(IRepository<CategorieProduit> repository)
        {
            _repository = repository;   
        }

        // GET: api/Categories
        /// <summary>
        /// Recupere la liste des categories.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategorieProduit>>> GetCategories()
        {
            return await _repository.GetAllAsync();
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
            var categorie = await _repository.GetByIdAsync(id);

            if (categorie == null)
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
        public async Task<IActionResult> PutJoueur(int id, CategorieProduit categorie)
        {

            if (id != categorie.Id)
            {
                return BadRequest();
            }

            try
            {
                var oldCategorie  = await _repository.GetByIdAsync(id);
                await _repository.UpdateAsync(oldCategorie.Value, categorie);
            }
            catch (Exception)
            {
                return NotFound();
            }

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
        public async Task<ActionResult<Joueur>> PostJoueur(CategorieProduit categorie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repository.AddAsync(categorie);

            return CreatedAtAction("GetJoueurById", new { id = categorie.Id }, categorie);
        }

        // DELETE: api/Categories/5
        /// <summary>
        /// Supprime la catégorie existant.
        /// </summary>
        /// <param name="id">L'ID de la catégorie à supprimer.</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteJoueur(int id)
        {
            var result = await _repository.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(result.Value);

            return NoContent();
        }

    }
}
