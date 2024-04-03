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

        private readonly FifaDbContext _context;

        public VotesController(FifaDbContext context)
        {
            _context = context;
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
            return await _context.VoteUtilisateurs.ToListAsync();
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
            var voteUtilisateur = await _context.VoteUtilisateurs.FindAsync(iduser, idtheme);

            if (voteUtilisateur == null)
            {
                return NotFound();
            }

            return voteUtilisateur;
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
            if (iduser != vote.IdUtilisateur || idtheme != vote.IdTheme)
            {
                return BadRequest();
            }

            try
            {
                await _context.UpdateEntity(vote);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VoteUtilisateurExists(idtheme, iduser))
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
            if(!await IsVoteValid(vote))
                return BadRequest();

            await _context.VoteUtilisateurs.AddAsync(vote);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VoteUtilisateurExists(vote.IdTheme, vote.IdUtilisateur))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

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
            var voteUtilisateur = await _context.VoteUtilisateurs.FindAsync(iduser, idtheme);
            if (voteUtilisateur == null)
            {
                return NotFound();
            }

            _context.VoteUtilisateurs.Remove(voteUtilisateur);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VoteUtilisateurExists(int idtheme, int iduser)
        {
            return (_context.VoteUtilisateurs?.Any(e => e.IdUtilisateur == iduser && e.IdTheme == idtheme)).GetValueOrDefault();
        }

        private async Task<bool> IsVoteValid(VoteUtilisateur vote)
        {
            return await _context.ThemeVoteJoueurs.Where(t => t.IdTheme == vote.IdTheme 
                && (t.IdJoueur == vote.IdJoueur1 
                    || t.IdJoueur == vote.IdJoueur2 
                    || t.IdJoueur == vote.IdJoueur3))
                .CountAsync() == 3;
        }
    }
}
