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
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class CompetitionsController : ControllerBase
    {
        private readonly IManagerCompetition _manager;

        public CompetitionsController(IManagerCompetition manager)
        {
            _manager = manager;
        }

        // GET: api/Competitions
        /// <summary>
        /// Retourne la liste des compétitions.
        /// </summary>
        /// <remarks>NOTE: La requête filtre les instances en fonction du niveau de permission.</remarks>
        /// <returns>La liste des compétitions.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Competition>>> GetCompetitions()
        {
            bool seeAll = await this.MatchPolicyAsync(ProduitsController.SEE_POLICY);
            return Ok(await _manager.GetAll(!seeAll));
        }

        // GET: api/Competitions/5
        /// <summary>
        /// Retourne une compétition.
        /// </summary>
        /// <remarks>NOTE: La requête filtre les instances en fonction du niveau de permission.</remarks>
        /// <param name="id">L'id de la compétition.</param>
        /// <returns>La compétition recherchée.</returns>
        /// <response code="404">La compétition recherchée n'existe pas ou a été filtrée.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Competition>> GetCompetition(int id)
        {
            bool seeAll = await this.MatchPolicyAsync(ProduitsController.SEE_POLICY);
            var competition = await _manager.GetById(id, !seeAll);

            if (competition == null) return NotFound();
            return Ok(competition);
        }

        // PUT: api/Competitions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie une compétition.
        /// </summary>
        /// <param name="id">L'id de la compétition.</param>
        /// <param name="competition">Les nouvelles informations de la compétition.</param>
        /// <remarks>NOTE: Requiert les droits d'édition de produit.</remarks>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La compétition recherchée n'existe pas.</response>
        /// <response code="400">Les nouvelles informations de la compétition sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = ProduitsController.EDIT_POLICY)]
        public async Task<IActionResult> PutCompetition(int id, Competition competition)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (id != competition.Id)
            {
                return BadRequest();
            }

            if (!await _manager.Exists(id))
            {
                return NotFound();
            }

            await _manager.Update(competition);
            await _manager.Save();

            return NoContent();
        }

        // POST: api/Competitions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute une nouvelle compétition.
        /// </summary>
        /// <param name="competition">La compétition à ajouter.</param>
        /// <remarks>NOTE: Requiert les droits d'ajout de produit.</remarks>
        /// <returns>La nouvelle compétition.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">La nouvelle compétition est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = ProduitsController.ADD_POLICY)]
        public async Task<ActionResult<Competition>> PostCompetition(Competition competition)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _manager.Add(competition);
            await _manager.Save();

            return CreatedAtAction("GetCompetition", new { id = competition.Id }, competition);
        }

        // DELETE: api/Competitions/5
        /// <summary>
        /// Supprime une compétition.
        /// </summary>
        /// <param name="id">L'id de la compétition à supprimer.</param>
        /// <remarks>NOTE: Requiert les droits de suppression de produit.</remarks>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">La compétition recherchée n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = ProduitsController.DELETE_POLICY)]
        public async Task<IActionResult> DeleteCompetition(int id)
        {
            var competition = await _manager.GetById(id, false);
            if (competition == null)
            {
                return NotFound();
            }

            await _manager.Delete(competition);
            await _manager.Save();

            return NoContent();
        }
    }
}
