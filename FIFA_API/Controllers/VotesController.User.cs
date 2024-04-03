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

        [HttpPost("me")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [Authorize(Policy = Policies.User)]
        [VerifiedEmail]
        public async Task<ActionResult<VoteUtilisateur>> CreateVote(VoteUtilisateur vote)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            var theme = await _context.ThemeVotes.FindAsync(vote.IdTheme);
            if (theme is null || !theme.Visible) return NotFound();

            vote.IdUtilisateur = user.Id;
            return await PostVoteUtilisateur(vote);
        }

        [HttpPut("me")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Policy = Policies.User)]
        [VerifiedEmail]
        public async Task<IActionResult> UpdateMyVote(VoteUtilisateur vote)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            var theme = await _context.ThemeVotes.FindAsync(vote.IdTheme);
            if (theme is null || !theme.Visible) return NotFound();

            vote.IdUtilisateur = user.Id;
            return await PutVoteUtilisateur(vote.IdTheme, user.Id, vote);
        }
    }
}
