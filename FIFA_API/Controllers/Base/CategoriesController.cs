using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class CategoriesController : ControllerBase
    {
        private readonly IManagerCategorieProduit _manager;

        public CategoriesController(IManagerCategorieProduit manager)
        {
            _manager = manager;
        }

        // GET: api/Categories
        /// <summary>
        /// Retourne la liste des catégories de produit.
        /// </summary>
        /// <returns>La liste des catégories visibles.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategorieProduit>>> GetCategorieProduits()
        {
            bool seeAll = await this.MatchPolicyAsync(ProduitsController.SEE_POLICY);
            return Ok(await _manager.GetAll(!seeAll));
        }

        // GET: api/Categories/5
        /// <summary>
        /// Retourne la catégorie de produit avec l'id passé.
        /// </summary>
        /// <param name="id">L'id de la catégorie.</param>
        /// <returns>La catégorie de produit.</returns>
        /// <response code="404">La catégorie recherchée n'existe pas.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<CategorieProduit>> GetCategorieProduit(int id)
        {
            bool seeAll = await this.MatchPolicyAsync(ProduitsController.SEE_POLICY);
            var categorieProduit = await _manager.GetById(id, !seeAll);

            if (categorieProduit == null) return NotFound();
            return Ok(categorieProduit);
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie une catégorie de produit.
        /// </summary>
        /// <param name="id">L'id de la catégorie.</param>
        /// <param name="categorieProduit">Les nouvelles informations de la catégorie de produit.</param>
        /// <returns>Réponse HTTP</returns>
        /// <remarks>NOTE: Requiert les droits d'édition de produit.</remarks>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La catégorie recherchée n'existe pas.</response>
        /// <response code="400">La catégorie est invalide.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = ProduitsController.EDIT_POLICY)]
        public async Task<IActionResult> PutCategorieProduit(int id, CategorieProduit categorieProduit)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            if (id != categorieProduit.Id)
            {
                return BadRequest();
            }

            if (!await _manager.Exists(id))
            {
                return NotFound();
            }

            await _manager.Update(categorieProduit);
            await _manager.Save();

            return NoContent();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute une catégorie de produit.
        /// </summary>
        /// <param name="categorieProduit">La catégorie de produit à ajouter.</param>
        /// <returns>La catégorie de produit ajoutée.</returns>
        /// <remarks>NOTE: Requiert les droits d'ajout de produit.</remarks>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">La catégorie est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = ProduitsController.ADD_POLICY)]
        public async Task<ActionResult<CategorieProduit>> PostCategorieProduit(CategorieProduit categorieProduit)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _manager.Add(categorieProduit);
            await _manager.Save();

            return CreatedAtAction("GetCategorieProduit", new { id = categorieProduit.Id }, categorieProduit);
        }

        // DELETE: api/Categories/5
        /// <summary>
        /// Supprime une catégorie de produit.
        /// </summary>
        /// <param name="id">L'id de la catégorie de produit à supprimer.</param>
        /// <returns>Réponse HTTP</returns>
        /// <remarks>NOTE: Requiert les droits de suppression de produit.</remarks>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La catégorie recherchée n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = ProduitsController.DELETE_POLICY)]
        public async Task<IActionResult> DeleteCategorieProduit(int id)
        {
            var categorieProduit = await _manager.GetById(id, false);
            if (categorieProduit == null)
            {
                return NotFound();
            }

            await _manager.Delete(categorieProduit);
            await _manager.Save();

            return NoContent();
        }
    }
}
