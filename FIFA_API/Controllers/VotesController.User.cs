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
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<IEnumerable<VoteUtilisateur>>> GetMyVotes()
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            return Ok(user.Votes);
        }

        [HttpGet("me/{idtheme}")]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<VoteUtilisateur>> GetMyVote(int idtheme)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            return await GetVoteUtilisateur(idtheme, user.Id);
        }

        [HttpPost("me")]
        [Authorize(Policy = Policies.User)]
        public async Task<ActionResult<VoteUtilisateur>> CreateVote(VoteUtilisateur vote)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            vote.IdUtilisateur = user.Id;
            return await PostVoteUtilisateur(vote);
        }

        [HttpPut("me")]
        [Authorize(Policy = Policies.User)]
        public async Task<IActionResult> UpdateMyVote(VoteUtilisateur vote)
        {
            Utilisateur? user = await this.UtilisateurAsync();
            if (user is null) return Unauthorized();

            vote.IdUtilisateur = user.Id;
            return await PutVoteUtilisateur(vote.IdTheme, user.Id, vote);
        }
    }
}
