using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Models;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using FIFA_API.Repositories.Contracts;

namespace FIFA_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public partial class ThemeVotesController : ControllerBase
    {
        /// <summary>
        /// Le nom de la <see cref="AuthorizationPolicy"/> requise pour les actions de modifications.
        /// </summary>
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly IUnitOfWorkVote _uow;

        public ThemeVotesController(IUnitOfWorkVote uow)
        {
            _uow = uow;
        }

        // GET: api/ThemeVotes
        /// <summary>
        /// Retourne la liste des thèmes de vote.
        /// </summary>
        /// <remarks>NOTE: La requête filtre les instances en fonction du niveau de permission.</remarks>
        /// <returns>La liste des thèmes de vote.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ThemeVote>>> GetThemeVotes()
        {
            bool seeAll = await this.MatchPolicyAsync(MANAGER_POLICY);
            return Ok(await _uow.Themes.GetAll(!seeAll));
        }

        // GET: api/ThemeVotes/5
        /// <summary>
        /// Retourne un thème de vote.
        /// </summary>
        /// <remarks>NOTE: La requête filtre les instances en fonction du niveau de permission.</remarks>
        /// <param name="id">L'id du thème de vote recherché.</param>
        /// <returns>Le thème de vote recherché.</returns>
        /// <response code="404">Le thème de vote recherché n'existe pas ou a été filtré.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ThemeVote>> GetThemeVote(int id)
        {
            bool seeAll = await this.MatchPolicyAsync(MANAGER_POLICY);
            var themeVote = await _uow.Themes.GetById(id, !seeAll);

            if (themeVote == null) return NotFound();
            return Ok(themeVote);
        }

        // PUT: api/ThemeVotes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie un thème de vote.
        /// </summary>
        /// <param name="id">L'id du thème de vote à modifier.</param>
        /// <param name="themeVote">Les nouvelles informations du thème de vote.</param>
        /// <remarks>NOTE: Requiert les droits d'édition de produit.</remarks>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le thème de vote recherché n'existe pas.</response>
        /// <response code="400">Les nouvelles informations du thème de vote sont invalides.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutThemeVote(int id, ThemeVote themeVote)
        {
            if(!ModelState.IsValid) return BadRequest(ModelState);

            if (id != themeVote.Id)
            {
                return BadRequest();
            }

            if (!await _uow.Themes.Exists(id))
            {
                return NotFound();
            }

            await _uow.Themes.Update(themeVote);
            await _uow.SaveChanges();

            return NoContent();
        }

        // POST: api/ThemeVotes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute un thème de vote.
        /// </summary>
        /// <param name="themeVote">Le thème de vote à ajouter.</param>
        /// <remarks>NOTE: Requiert les droits d'ajout de produit.</remarks>
        /// <returns>Le nouveau thème de vote.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">Le nouveau thème de vote est invalide.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<ThemeVote>> PostThemeVote(ThemeVote themeVote)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _uow.Themes.Add(themeVote);
            await _uow.SaveChanges();

            return CreatedAtAction("GetThemeVote", new { id = themeVote.Id }, themeVote);
        }

        // DELETE: api/ThemeVotes/5
        /// <summary>
        /// Supprime un thème de vote.
        /// </summary>
        /// <param name="id">L'id du thème de vote recherché.</param>
        /// <remarks>NOTE: Requiert les droits de suppression de produit.</remarks>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le thème de vote recherché n'existe pas.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteThemeVote(int id)
        {
            var themeVote = await _uow.Themes.GetById(id);
            if (themeVote == null)
            {
                return NotFound();
            }

            await _uow.Themes.Delete(themeVote);
            await _uow.SaveChanges();

            return NoContent();
        }
    }
}
