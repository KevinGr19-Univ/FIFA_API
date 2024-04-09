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
    public partial class VotesController : ControllerBase
    {
        /// <summary>
        /// Le nom de la <see cref="AuthorizationPolicy"/> requise pour les actions de modifications.
        /// </summary>
        public const string MANAGER_POLICY = Policies.Admin;

        private readonly IUnitOfWorkVote _uow;

        public VotesController(IUnitOfWorkVote uow)
        {
            _uow = uow;
        }

        // GET: api/Votes
        /// <summary>
        /// Retourne la liste des votes.
        /// </summary>
        /// <returns>La liste des votes.</returns>
        /// <response code="401">Accès refusé.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<IEnumerable<VoteUtilisateur>>> GetVoteUtilisateurs()
        {
            return Ok(await _uow.Votes.GetAll());
        }

        // GET: api/Votes/5
        /// <summary>
        /// Retourne le vote avec l'id passé.
        /// </summary>
        /// <param name="idtheme">L'id du thème de vote.</param>
        /// <param name="iduser">L'id de l'utilisateur.</param>
        /// <returns>Le vote recherché.</returns>
        /// <response code="404">Le vote recherché n'existe pas.</response>
        [HttpGet("{idtheme}/{iduser}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<VoteUtilisateur>> GetVoteUtilisateur(int idtheme, int iduser)
        {
            var voteUtilisateur = await _uow.Votes.GetById(iduser, idtheme);

            if (voteUtilisateur == null)
            {
                return NotFound();
            }

            return Ok(voteUtilisateur);
        }

        // PUT: api/Votes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifie un vote avec les informations passés.
        /// </summary>
        /// <remarks>NOTE: Cette méthode ne devrait pas être utilisée pour un usage commun.</remarks>
        /// <param name="idtheme">L'id du thème de vote.</param>
        /// <param name="iduser">L'id de l'utilisateur.</param>
        /// <param name="vote">Les nouvelles informations du vote.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le vote recherché n'existe pas.</response>
        /// <response code="400">Les nouvelles informations du vote sont invalides.</response>
        [HttpPut("{idtheme}/{iduser}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> PutVoteUtilisateur(int idtheme, int iduser, VoteUtilisateur vote)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (iduser != vote.IdUtilisateur || idtheme != vote.IdTheme)
            {
                return BadRequest();
            }

            if (!await _uow.Votes.Exists(idtheme, iduser))
            {
                return NotFound();
            }

            if (!await _uow.IsVoteValid(vote))
                return BadRequest();

            await _uow.Votes.Update(vote);
            await _uow.SaveChanges();

            return NoContent();
        }

        // POST: api/Votes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Ajoute un vote.
        /// </summary>
        /// <remarks>NOTE: Cette méthode ne devrait pas être utilisée pour un usage commun.</remarks>
        /// <param name="vote">Le vote à ajotuer.</param>
        /// <returns>Le vote ajouté.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="400">Le vote est invalide.</response>
        /// <response code="409">Un vote existe déjà pour le thème de vote et l'utilisateur donnés.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<ActionResult<VoteUtilisateur>> PostVoteUtilisateur(VoteUtilisateur vote)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (await _uow.Votes.Exists(vote.IdTheme, vote.IdUtilisateur))
            {
                return Conflict();
            }

            if (!await _uow.IsVoteValid(vote))
                return BadRequest();

            await _uow.Votes.Add(vote);
            await _uow.SaveChanges();

            return CreatedAtAction("GetVoteUtilisateur", new { idtheme = vote.IdTheme, iduser = vote.IdUtilisateur }, vote);
        }

        // DELETE: api/Votes/5
        /// <summary>
        /// Supprime un vote.
        /// </summary>
        /// <param name="idtheme">L'id du thème de vote.</param>
        /// <param name="iduser">L'id de l'utilisateur.</param>
        /// <returns>Réponse HTTP</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le vote recherché n'existe pas.</response>
        [HttpDelete("{idtheme}/{iduser}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = MANAGER_POLICY)]
        public async Task<IActionResult> DeleteVoteUtilisateur(int idtheme, int iduser)
        {
            var voteUtilisateur = await _uow.Votes.GetById(iduser, idtheme);
            if (voteUtilisateur == null)
            {
                return NotFound();
            }

            await _uow.Votes.Delete(voteUtilisateur);
            await _uow.SaveChanges();

            return NoContent();
        }
    }
}
