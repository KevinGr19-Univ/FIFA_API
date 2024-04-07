using FIFA_API.Authorization;
using FIFA_API.Models;
using FIFA_API.Models.EntityFramework;
using FIFA_API.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIFA_API.Controllers
{
    public partial class VotesController
    {
        /// <summary>
        /// Retourne la liste des votes de l'utilisateur.
        /// </summary>
        /// <returns>La liste des votes de l'utilisateur.</returns>
        /// <response code="401">Accès refusé.</response>
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = Policies.User)]
        [VerifiedEmail]
        public async Task<ActionResult<IEnumerable<VoteUtilisateur>>> GetMyVotes()
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            return Ok(user.Votes);
        }

        /// <summary>
        /// Retourne un vote de l'utilisateur.
        /// </summary>
        /// <param name="idtheme">L'id du thème de vote.</param>
        /// <returns>Un vote de l'utilisateur.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le vote recherché n'existe pas.</response>
        [HttpGet("me/{idtheme}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Policy = Policies.User)]
        [VerifiedEmail]
        public async Task<ActionResult<VoteUtilisateur>> GetMyVote(int idtheme)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            return await GetVoteUtilisateur(idtheme, user.Id);
        }

        /// <summary>
        /// Publie un vote.
        /// </summary>
        /// <param name="vote">Le nouveau vote.</param>
        /// <returns>Le nouveau vote.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le thème de vote recherché n'existe pas.</response>
        /// <response code="400">Le nouveau vote est invalide.</response>
        [HttpPost("me")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = Policies.User)]
        [VerifiedEmail]
        public async Task<ActionResult<VoteUtilisateur>> CreateVote(VoteUtilisateur vote)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            var theme = await _manager.ThemeVotes.FindAsync(vote.IdTheme);
            if (theme is null || !theme.Visible) return NotFound();

            vote.IdUtilisateur = user.Id;
            return await PostVoteUtilisateur(vote);
        }

        /// <summary>
        /// Met à jour un vote de l'utilisateur.
        /// </summary>
        /// <param name="vote">Les nouvelles informations du vote à mettre à jour.</param>
        /// <returns>Réponse HTTP.</returns>
        /// <response code="401">Accès refusé.</response>
        /// <response code="404">Le vote recherché n'existe pas.</response>
        /// <response code="400">Les nouvelles informations du vote sont invalides.</response>
        [HttpPut("me")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = Policies.User)]
        [VerifiedEmail]
        public async Task<IActionResult> UpdateMyVote(VoteUtilisateur vote)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            var theme = await _manager.ThemeVotes.FindAsync(vote.IdTheme);
            if (theme is null || !theme.Visible) return NotFound();

            vote.IdUtilisateur = user.Id;
            return await PutVoteUtilisateur(vote.IdTheme, user.Id, vote);
        }
    }
}
