using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using FIFA_API.Models;
using Microsoft.AspNetCore.Authorization;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class TypesLivraisonController : ControllerBase
    {
        /// <summary>
        /// Le nom de la <see cref="AuthorizationPolicy"/> requise pour les actions de modifications.
        /// </summary>
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly IManagerTypeLivraison _manager;

        public TypesLivraisonController(IManagerTypeLivraison manager)
        {
            _manager = manager;
        }

        // GET: api/TypesLivraison
        /// <summary>
        /// Retourne la liste des types de livraison.
        /// </summary>
        /// <returns>La liste des types de livraison.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TypeLivraison>>> GetTypeLivraisons()
        {
            return Ok(await _manager.GetAll());
        }

        // GET: api/TypesLivraison/5
        /// <summary>
        /// Retourne le type de livraison avec l'id passé.
        /// </summary>
        /// <param name="id">L'id du type de livraison.</param>
        /// <returns>Le type de livraison recherché.</returns>
        /// <response code="404">Le type de livraison recherché n'existe pas.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<TypeLivraison>> GetTypeLivraison(int id)
        {
            var typeLivraison = await _manager.GetById(id);

            if (typeLivraison is null)
            {
                return NotFound();
            }

            return typeLivraison;
        }

        // PUT: api/TypesLivraison/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie un type de livraison avec les informations passés.
        /// </summary>
        /// <param name="id">L'id du type de livraison à modifier.</param>
        /// <param name="typeLivraison">Les nouvelles informations du type de livraison.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le type de livraison recherché n'existe pas.</response>
        /// <response code="400">Les nouvelles informations du type de livraison sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutTypeLivraison(int id, TypeLivraison typeLivraison)
        {
            if (id != typeLivraison.Id)
            {
                return BadRequest();
            }

            try
            {
                await _manager.Update(typeLivraison);
                await _manager.Save();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _manager.Exists(id))
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

        // POST: api/TypesLivraison
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute un type de livraison.
        /// </summary>
        /// <param name="typeLivraison">Le type de livraison à ajotuer.</param>
        /// <returns>Le type de livraison ajouté.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">Le type de livraison est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<TypeLivraison>> PostTypeLivraison(TypeLivraison typeLivraison)
        {
            await _manager.Add(typeLivraison);
            await _manager.Save();

            return CreatedAtAction("GetTypeLivraison", new { id = typeLivraison.Id }, typeLivraison);
        }

        // DELETE: api/TypesLivraison/5
        /// <summary>
        /// Supprime un trophée.
        /// </summary>
        /// <param name="id">L'id du trophée à supprimer.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le trophée recherché n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteTypeLivraison(int id)
        {
            var typeLivraison = await _manager.GetById(id);
            if (typeLivraison == null)
            {
                return NotFound();
            }

            await _manager.Delete(typeLivraison);
            await _manager.Save();

            return NoContent();
        }
    }
}
