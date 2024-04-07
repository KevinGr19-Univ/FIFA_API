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

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class NationsController : ControllerBase
    {
        private readonly IManagerNation _manager;

        public NationsController(IManagerNation manager)
        {
            _manager = manager;
        }

        // GET: api/Nations
        /// <summary>
        /// Retourne la liste des nations.
        /// </summary>
        /// <remarks>NOTE: La requête filtre les instances en fonction du niveau de permission.</remarks>
        /// <returns>La liste des nations.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Nation>>> GetNations()
        {
            bool seeAll = await this.MatchPolicyAsync(ProduitsController.SEE_POLICY);
            return Ok(await _manager.GetAll(!seeAll));
        }

        // GET: api/Nations/5
        /// <summary>
        /// Retourne une nation.
        /// </summary>
        /// <remarks>NOTE: La requête filtre les instances en fonction du niveau de permission.</remarks>
        /// <param name="id">L'id de la nation.</param>
        /// <returns>La nation recherchée.</returns>
        /// <response code="404">La nation recherchée n'existe pas ou a été filtrée.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Nation>> GetNation(int id)
        {
            bool seeAll = await this.MatchPolicyAsync(ProduitsController.SEE_POLICY);
            var nation = await _manager.GetById(id, !seeAll);

            if (nation == null) return NotFound();
            return nation;
        }

        // PUT: api/Nations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie une nation.
        /// </summary>
        /// <param name="id">L'id de la nation.</param>
        /// <param name="nation">Les nouvelles informations de la nation.</param>
        /// <remarks>NOTE: Requiert les droits d'édition de produit.</remarks>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La nation recherchée n'existe pas.</response>
        /// <response code="400">Les nouvelles informations de la nation sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = ProduitsController.EDIT_POLICY)]
        public async Task<IActionResult> PutNation(int id, Nation nation)
        {
            if (id != nation.Id)
            {
                return BadRequest();
            }

            try
            {
                await _manager.Update(nation);
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

        // POST: api/Nations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute une nouvelle nation.
        /// </summary>
        /// <param name="nation">La nation à ajouter.</param>
        /// <remarks>NOTE: Requiert les droits d'ajout de produit.</remarks>
        /// <returns>La nouvelle nation.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">La nouvelle nation est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = ProduitsController.ADD_POLICY)]
        public async Task<ActionResult<Nation>> PostNation(Nation nation)
        {
            await _manager.Add(nation);
            await _manager.Save();

            return CreatedAtAction("GetNation", new { id = nation.Id }, nation);
        }

        // DELETE: api/Nations/5
        /// <summary>
        /// Supprime une nation.
        /// </summary>
        /// <param name="id">L'id de la nation à supprimer.</param>
        /// <remarks>NOTE: Requiert les droits de suppression de produit.</remarks>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La nation recherchée n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = ProduitsController.DELETE_POLICY)]
        public async Task<IActionResult> DeleteNation(int id)
        {
            var nation = await _manager.GetById(id, false);
            if (nation == null)
            {
                return NotFound();
            }

            await _manager.Delete(nation);
            await _manager.Save();

            return NoContent();
        }
    }
}
